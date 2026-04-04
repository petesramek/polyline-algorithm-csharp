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

    select.addEventListener('change', function () {
      var targetVersion = select.value;
      if (!targetVersion) return;

      var newPathname;
      if (currentVersion && relativePath) {
        newPathname = window.location.pathname.replace(
          '/' + currentVersion + '/',
          '/' + targetVersion + '/'
        );
      } else {
        newPathname = window.location.pathname.replace(/\/$/, '') + '/' + targetVersion + '/';
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
