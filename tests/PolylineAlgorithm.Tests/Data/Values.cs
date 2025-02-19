//
// Copyright © Pete Sramek. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

namespace PolylineAlgorithm.Tests.Data;

using System.Collections.Generic;

/// <summary>
/// Defines default values and objects used for testing purposes
/// </summary>
internal static class Values {
    private static readonly Random _random = new(DateTime.Now.Millisecond);

    /// <summary>
    /// Defines default decoded values and objects udśed for testing purposes
    /// </summary>
    public static class Coordinates {
        /// <summary>
        /// Defines empty range of coordinates. Equals to decoded <seealso cref="Polyline.Empty"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Empty = [];

        /// <summary>
        /// Defines range of invalid coordinates. Equals to decoded <seealso cref="Polyline.Invalid"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Invalid = [
            new(149.47383, 259.06250),
            new(-158.37407, 225.31250),
            new(152.99363, -220.93750),
            new(-144.49024, -274.37500)
        ];

        /// <summary>
        /// Defines range of valid coordinates. Equals to decoded <seealso cref="Polyline.Valid"/>
        /// </summary>
        public static readonly IEnumerable<Coordinate> Valid = [
            new(41.45450,-120.00601),
            new(-8.48144,-86.82596),
            new(39.26923,103.12430),
            new(17.55738,44.22419),
            new(89.33118,-11.80985),
            new(-79.83895,-134.39806),
            new(-75.30707,-7.32566),
            new(20.50667,-46.45419),
            new(-9.59742,64.81665),
            new(-25.99973,175.77830),
            new(56.39255,-138.32482),
            new(84.78677,-154.10794),
            new(0.59605,-73.98923),
            new(-31.10644,-8.43068),
            new(-30.43549,148.76201),
            new(-82.41523,20.21469),
            new(71.24263,62.19579),
            new(68.80790,130.99556),
            new(-28.31160,-44.33518),
            new(-81.54954,47.29398),
            new(21.58666,-117.97945),
            new(-51.63946,-100.44378),
            new(76.10919,-4.20415),
            new(-74.77389,85.23021),
            new(60.90186,99.72645),
            new(-69.12375,-175.30625),
            new(41.29931,60.63728),
            new(3.81173,8.87839),
            new(77.61927,-129.54681),
            new(-15.46311,-178.46262),
            new(-85.88171,-82.23031),
            new(-32.93202,147.11694),
            new(80.14496,118.14265),
            new(31.90642,-114.25535),
            new(13.47895,-76.08751),
            new(34.58285,-166.88570),
            new(15.60940,131.46776),
            new(-74.71474,114.29821),
            new(71.23988,73.99397),
            new(19.24349,-45.33576),
            new(-19.44923,67.37344),
            new(86.38216,-106.97476),
            new(-11.60353,136.93313),
            new(-45.90959,124.43965),
            new(-75.84455,-113.28929),
            new(-53.55922,-168.33531),
            new(-31.46850,-92.56667),
            new(-53.81225,-130.67001),
            new(-71.37591,7.14206),
            new(71.66438,-132.79779),
            new(-17.16104,-169.31147),
            new(-17.11500,-162.19145),
            new(-18.73571,148.45104),
            new(-79.33707,-14.05000),
            new(3.02265,-163.54949),
            new(-65.22814,116.24980),
            new(-43.45029,118.62447),
            new(12.70248,15.41610),
            new(-85.01689,-69.63517),
            new(-16.65658,-86.55818),
            new(-3.56486,-166.06182),
            new(-40.31288,-82.55762),
            new(-60.42828,44.87286),
            new(21.68178,-157.36765),
            new(-87.21960,-25.14416),
            new(-79.57482,-93.30716),
            new(39.49740,-25.92495),
            new(-25.90437,14.17541),
            new(47.60480,168.70333),
            new(42.88459,171.57119),
            new(-39.61456,-14.00303),
            new(-75.32873,107.34138),
            new(75.67415,-54.36983),
            new(74.40329,-41.94333),
            new(-54.65484,-101.22314),
            new(-43.80323,-149.93200),
            new(-84.55216,-140.98704),
            new(38.36040,143.13161),
            new(54.85902,28.67174),
            new(83.24289,40.04888),
            new(35.08649,-123.31772),
            new(44.90926,-85.63977),
            new(-50.08800,2.38870),
            new(29.19869,-67.91889),
            new(-36.60715,167.63141),
            new(11.78538,46.55898),
            new(49.21703,117.05418),
            new(58.71289,-146.04760),
            new(41.16914,-151.82725),
            new(-82.59634,-64.87697),
            new(-56.25088,105.80112),
            new(63.50979,-12.39384),
            new(8.49684,-45.42851),
            new(-39.01781,-4.08632),
            new(-7.40331,146.52996),
            new(-74.79198,12.50727),
            new(57.55565,56.49742),
            new(-68.86080,-165.10417),
            new(-77.15836,-19.18719),
            new(88.82714,74.26587),
        ];
    }

    /// <summary>
    /// Defines default encoded values and objects udśed for testing purposes
    /// </summary>
    public static class Polyline {
        /// <summary>
        /// Defines empty string of polyline encoded coordinates. Equals to encoded <seealso cref="Coordinates.Empty"/>
        /// </summary>
        public static readonly string Empty = string.Empty;

        /// <summary>
        /// Defines polyline encoded range of invalid coordinates. Equals to encoded <seealso cref="Coordinates.Invalid"/>
        /// </summary>
        public static readonly string Invalid = "mnc~Qsm_ja@";

        /// <summary>
        /// Defines polyline encoded range of invalid coordinates. Equals to encoded <seealso cref="Coordinates.Invalid"/>
        /// </summary>
        public static readonly string Malformed = "d";

        /// <summary>
        /// Defines polyline encoded range of valid coordinates. Equals to encoded <seealso cref="Coordinates.Valid"/>
        /// </summary>
        public static readonly string Valid = @"sq_|Fptm{UrbxoHinoiEuhmbHctjfc@`rocCt|nfJgiauLvc_uIh`pg_@h_fkVgctZoyqfW{rxgQhhymFpuvvDwqcfTlqbcBiegdTwf{uNngc|z@{vhlDnsi_B~nz`O}d_hNp{n`E}kcoKm`bCiul~\jhg|Hv{qoWshzh\{lf_G`pzMqm|bLzswoQbhcm`@b`}cIgignPgxntR|}vo^f~|}L}|_jBa|ujWuxkjQfj|w[wsz`Pmdb{XohnwA`srxWjitms@c_~`Tava_l@jxxcF`d|zHcpnaMnd{kYzccwPxzpiHfsxlL}jjjQqvdbIikyvj@cjdqTh`zoDzqleHnfmik@tbnoB_t}gFkzx_Ct_eiP`wxrBcd_|w@zlhfPtlxgBkwyyZn|~tFlpj|HxqiwUnddkFoo|nTee}dSfkcg`@py`uQiguom@zkkpEfcgkAntuuDzl~il@ir_gCrd~nI_ryeC_qmmMl_kgCz`qgFzkejBmlchYyp`hZ`_cuYzuc}Onqz}Ew~Gcsmj@lp{Hqj_gz@ne{pJnny~]g{tuNxbno[lfq_Lqhwjt@qn|cCuxnMyivuIh{|tR`ylsQlqbfO}rf`LxghfBg~{nAv`gdNbjh_Fglt|NfxwyBowwhW{bdtNdbkqe@rxtwSy{_fX{btm@va`_LkhwuUyqgzK`xdnKgbwsFigt_Mofdn\h|x[ccoPtbpvNz}skb@pl~xEqascV_wsx[`f_z]zewFs`zjAhturWxayhJqmfaAjmhhHxwuwF_aru@ojemVq|beu@kkucBdmryTevflDcbmdAnp|dHfpbd^io}z@e~}dFzcybQ}`hxOyt|bNl}blLnuspKk|t|k@itjfHt}}aVyzmcF_rgmLct}x@bazdq@loajBxygb@f}krVgnuqOcrx_Daqvp_@ew}yUn}kpU|uwnItashEpe_aHusi{Fsu}_Ewfhv[dzhzKxh_qXucxfXmynkGxuqbW|ppgi@vrsq@clryZk`bt^spkyP";
    }

    public static class MalformedPolylineException {
        public static readonly int Position = _random.Next();
    }

    public static class InvalidCoordinateException {
        public static readonly Coordinate Valid = new(_random.NextDouble(), _random.NextDouble());

        public static readonly Coordinate Invalid = new(double.MinValue, double.MaxValue);
    }

    public static class InvalidReaderStateException {
        public static readonly int Position = _random.Next();
        public static readonly int Length = Position;
    }

    public static class InvalidWriterStateException {
        public static readonly int Position = _random.Next();
        public static readonly int Length = Position;
    }
}
