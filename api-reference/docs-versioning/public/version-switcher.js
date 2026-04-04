(function () {
  'use strict';

  function getVersionFromPath(pathname) {
    var match = pathname.match(/\/(\d+\.\d+)\//);
    return match ? match[1] : null;
  }

  function getPathAfterVersion(pathname) {
    var match = pathname.match(/\/\d+\.\d+\/(.*)/);
    return match ? match[1] : '';
  }

  function getSiteRoot() {
    var meta = document.querySelector('meta[name="docfx:rel"]');
    return meta ? meta.getAttribute('content') : './';
  }

  var VERSION_PATTERN = /\/\d+(\.\d+)*\//;

  function updateNavLinks(version) {
    var navbar = document.getElementById('navbar');
    if (!navbar || !version) return;
    if (!/^\d+(\.\d+)*$/.test(version)) return;

    var links = navbar.querySelectorAll('a');
    links.forEach(function (link) {
      var absUrl;
      try {
        absUrl = new URL(link.href);
      } catch (e) {
        return;
      }
      if (absUrl.origin !== window.location.origin) return;
      var newPathname = absUrl.pathname.replace(VERSION_PATTERN, '/' + version + '/');
      if (newPathname !== absUrl.pathname) {
        link.setAttribute('href', absUrl.origin + newPathname + absUrl.search + absUrl.hash);
      }
    });
  }

  function initVersionPicker(versions, latest) {
    var select = document.getElementById('version-picker');
    if (!select) return;

    var currentVersion = getVersionFromPath(window.location.pathname);
    var relativePath = getPathAfterVersion(window.location.pathname);

    versions.forEach(function (v) {
      var option = document.createElement('option');
      option.value = v;
      option.textContent = v === latest ? 'v' + v + ' (latest)' : 'v' + v;
      if (v === currentVersion) {
        option.selected = true;
      }
      select.appendChild(option);
    });

    if (!currentVersion) {
      var placeholder = document.createElement('option');
      placeholder.value = '';
      placeholder.textContent = 'Select version';
      placeholder.selected = true;
      placeholder.disabled = true;
      select.insertBefore(placeholder, select.firstChild);
    }

    window.addEventListener('load', function () {
      updateNavLinks(currentVersion || latest);
    });

    select.addEventListener('change', function () {
      var targetVersion = select.value;
      if (!targetVersion) return;

      updateNavLinks(targetVersion);

      var newPathname;
      if (currentVersion && relativePath) {
        newPathname = window.location.pathname.replace(
          '/' + currentVersion + '/',
          '/' + targetVersion + '/'
        );
      } else {
        var siteRootPath = new URL(getSiteRoot(), window.location.href).pathname.replace(/\/$/, '');
        newPathname = siteRootPath + '/' + targetVersion + '/PolylineAlgorithm.html';
      }

      window.location.pathname = newPathname;
    });
  }

  function loadVersions() {
    var root = getSiteRoot();
    var url = root + 'versions.json';

    fetch(url)
      .then(function (r) {
        if (!r.ok) throw new Error('versions.json not found');
        return r.json();
      })
      .then(function (data) {
        initVersionPicker(data.versions, data.latest);
      })
      .catch(function () {
        var container = document.getElementById('version-picker-container');
        if (container) container.style.display = 'none';
      });
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', loadVersions);
  } else {
    loadVersions();
  }
}());
