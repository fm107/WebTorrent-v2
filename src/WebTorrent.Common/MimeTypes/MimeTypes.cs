﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace WebTorrent.Common.MimeTypes
{
  public static class MimeTypes
  {
    private static readonly Lazy<ReadOnlyDictionary<string, string>> LazyDict = new Lazy<ReadOnlyDictionary<string, string>>((Func<ReadOnlyDictionary<string, string>>) (() =>
    {
      string[] allExts = MimeTypes.ALL_EXTS;
      Func<string, string> func = (Func<string, string>) (e => e);
      Func<string, string> keySelector = null;
      return new ReadOnlyDictionary<string, string>((IDictionary<string, string>) ((IEnumerable<string>) allExts).ToDictionary<string, string, string>(keySelector, (Func<string, string>) (e => MimeTypes.LookupType(e))));
    }));
    private static readonly string[] ALL_EXTS = new string[1019]
    {
      "ez",
      "aw",
      "atom",
      "atomcat",
      "atomsvc",
      "ccxml",
      "cdmia",
      "cdmic",
      "cdmid",
      "cdmio",
      "cdmiq",
      "cu",
      "davmount",
      "dbk",
      "dssc",
      "xdssc",
      "ecma",
      "emma",
      "epub",
      "exi",
      "pfr",
      "woff",
      "gml",
      "gpx",
      "gxf",
      "stk",
      "ink",
      "inkml",
      "ipfix",
      "jar",
      "ser",
      "class",
      "js",
      "json",
      "jsonml",
      "lostxml",
      "hqx",
      "cpt",
      "mads",
      "mrc",
      "mrcx",
      "ma",
      "nb",
      "mb",
      "mathml",
      "mbox",
      "mscml",
      "metalink",
      "meta4",
      "mets",
      "mods",
      "m21",
      "mp21",
      "mp4s",
      "doc",
      "dot",
      "mxf",
      "bin",
      "dms",
      "lrf",
      "mar",
      "so",
      "dist",
      "distz",
      "pkg",
      "bpk",
      "dump",
      "elc",
      "deploy",
      "oda",
      "opf",
      "ogx",
      "omdoc",
      "onetoc",
      "onetoc2",
      "onetmp",
      "onepkg",
      "oxps",
      "xer",
      "pdf",
      "pgp",
      "asc",
      "sig",
      "prf",
      "p10",
      "p7m",
      "p7c",
      "p7s",
      "p8",
      "ac",
      "cer",
      "crl",
      "pkipath",
      "pki",
      "pls",
      "ai",
      "eps",
      "ps",
      "cww",
      "pskcxml",
      "rdf",
      "rif",
      "rnc",
      "rl",
      "rld",
      "rs",
      "gbr",
      "mft",
      "roa",
      "rsd",
      "rss",
      "rtf",
      "sbml",
      "scq",
      "scs",
      "spq",
      "spp",
      "sdp",
      "setpay",
      "setreg",
      "shf",
      "smi",
      "smil",
      "rq",
      "srx",
      "gram",
      "grxml",
      "sru",
      "ssdl",
      "ssml",
      "tei",
      "teicorpus",
      "tfi",
      "tsd",
      "plb",
      "psb",
      "pvb",
      "tcap",
      "pwn",
      "aso",
      "imp",
      "acu",
      "atc",
      "acutc",
      "air",
      "fcdt",
      "fxp",
      "fxpl",
      "xdp",
      "xfdf",
      "ahead",
      "azf",
      "azs",
      "azw",
      "acc",
      "ami",
      "apk",
      "cii",
      "fti",
      "atx",
      "mpkg",
      "m3u8",
      "swi",
      "iota",
      "aep",
      "mpm",
      "bmi",
      "rep",
      "cdxml",
      "mmd",
      "cdy",
      "cla",
      "rp9",
      "c4g",
      "c4d",
      "c4f",
      "c4p",
      "c4u",
      "c11amc",
      "c11amz",
      "csp",
      "cdbcmsg",
      "cmc",
      "clkx",
      "clkk",
      "clkp",
      "clkt",
      "clkw",
      "wbs",
      "pml",
      "ppd",
      "car",
      "pcurl",
      "dart",
      "rdz",
      "uvf",
      "uvvf",
      "uvd",
      "uvvd",
      "uvt",
      "uvvt",
      "uvx",
      "uvvx",
      "uvz",
      "uvvz",
      "fe_launch",
      "dna",
      "mlp",
      "dpg",
      "dfac",
      "kpxx",
      "ait",
      "svc",
      "geo",
      "mag",
      "nml",
      "esf",
      "msf",
      "qam",
      "slt",
      "ssf",
      "es3",
      "et3",
      "ez2",
      "ez3",
      "fdf",
      "mseed",
      "seed",
      "dataless",
      "gph",
      "ftc",
      "fm",
      "frame",
      "maker",
      "book",
      "fnc",
      "ltf",
      "fsc",
      "oas",
      "oa2",
      "oa3",
      "fg5",
      "bh2",
      "ddd",
      "xdw",
      "xbd",
      "fzs",
      "txd",
      "ggb",
      "ggt",
      "gex",
      "gre",
      "gxt",
      "g2w",
      "g3w",
      "gmx",
      "kml",
      "kmz",
      "gqf",
      "gqs",
      "gac",
      "ghf",
      "gim",
      "grv",
      "gtm",
      "tpl",
      "vcg",
      "hal",
      "zmm",
      "hbci",
      "les",
      "hpgl",
      "hpid",
      "hps",
      "jlt",
      "pcl",
      "pclxl",
      "sfd-hdstx",
      "mpy",
      "afp",
      "listafp",
      "list3820",
      "irm",
      "sc",
      "icc",
      "icm",
      "igl",
      "ivp",
      "ivu",
      "igm",
      "xpw",
      "xpx",
      "i2g",
      "qbo",
      "qfx",
      "rcprofile",
      "irp",
      "xpr",
      "fcs",
      "jam",
      "rms",
      "jisp",
      "joda",
      "ktz",
      "ktr",
      "karbon",
      "chrt",
      "kfo",
      "flw",
      "kon",
      "kpr",
      "kpt",
      "ksp",
      "kwd",
      "kwt",
      "htke",
      "kia",
      "kne",
      "knp",
      "skp",
      "skd",
      "skt",
      "skm",
      "sse",
      "lasxml",
      "lbd",
      "lbe",
      "123",
      "apr",
      "pre",
      "nsf",
      "org",
      "scm",
      "lwp",
      "portpkg",
      "mcd",
      "mc1",
      "cdkey",
      "mwf",
      "mfm",
      "flo",
      "igx",
      "mif",
      "daf",
      "dis",
      "mbk",
      "mqy",
      "msl",
      "plc",
      "txf",
      "mpn",
      "mpc",
      "xul",
      "cil",
      "cab",
      "xls",
      "xlm",
      "xla",
      "xlc",
      "xlt",
      "xlw",
      "xlam",
      "xlsb",
      "xlsm",
      "xltm",
      "eot",
      "chm",
      "ims",
      "lrm",
      "thmx",
      "cat",
      "stl",
      "ppt",
      "pps",
      "pot",
      "ppam",
      "pptm",
      "sldm",
      "ppsm",
      "potm",
      "mpp",
      "mpt",
      "docm",
      "dotm",
      "wps",
      "wks",
      "wcm",
      "wdb",
      "wpl",
      "xps",
      "mseq",
      "mus",
      "msty",
      "taglet",
      "nlu",
      "ntf",
      "nitf",
      "nnd",
      "nns",
      "nnw",
      "ngdat",
      "n-gage",
      "rpst",
      "rpss",
      "edm",
      "edx",
      "ext",
      "odc",
      "otc",
      "odb",
      "odf",
      "odft",
      "odg",
      "otg",
      "odi",
      "oti",
      "odp",
      "otp",
      "ods",
      "ots",
      "odt",
      "odm",
      "ott",
      "oth",
      "xo",
      "dd2",
      "oxt",
      "pptx",
      "sldx",
      "ppsx",
      "potx",
      "xlsx",
      "xltx",
      "docx",
      "dotx",
      "mgp",
      "dp",
      "esa",
      "pdb",
      "pqa",
      "oprc",
      "paw",
      "str",
      "ei6",
      "efif",
      "wg",
      "plf",
      "pbd",
      "box",
      "mgz",
      "qps",
      "ptid",
      "qxd",
      "qxt",
      "qwd",
      "qwt",
      "qxl",
      "qxb",
      "bed",
      "mxl",
      "musicxml",
      "cryptonote",
      "cod",
      "rm",
      "rmvb",
      "link66",
      "st",
      "see",
      "sema",
      "semd",
      "semf",
      "ifm",
      "itp",
      "iif",
      "ipk",
      "twd",
      "twds",
      "mmf",
      "teacher",
      "sdkm",
      "sdkd",
      "dxp",
      "sfs",
      "sdc",
      "sda",
      "sdd",
      "smf",
      "sdw",
      "vor",
      "sgl",
      "smzip",
      "sm",
      "sxc",
      "stc",
      "sxd",
      "std",
      "sxi",
      "sti",
      "sxm",
      "sxw",
      "sxg",
      "stw",
      "sus",
      "susp",
      "svd",
      "sis",
      "sisx",
      "xsm",
      "bdm",
      "xdm",
      "tao",
      "pcap",
      "cap",
      "dmp",
      "tmo",
      "tpt",
      "mxs",
      "tra",
      "ufd",
      "ufdl",
      "utz",
      "umj",
      "unityweb",
      "uoml",
      "vcx",
      "vsd",
      "vst",
      "vss",
      "vsw",
      "vis",
      "vsf",
      "wbxml",
      "wmlc",
      "wmlsc",
      "wtb",
      "nbp",
      "wpd",
      "wqd",
      "stf",
      "xar",
      "xfdl",
      "hvd",
      "hvs",
      "hvp",
      "osf",
      "osfpvg",
      "saf",
      "spf",
      "cmp",
      "zir",
      "zirz",
      "zaz",
      "vxml",
      "wgt",
      "hlp",
      "wsdl",
      "wspolicy",
      "7z",
      "abw",
      "ace",
      "dmg",
      "aab",
      "x32",
      "u32",
      "vox",
      "aam",
      "aas",
      "bcpio",
      "torrent",
      "blb",
      "blorb",
      "bz",
      "bz2",
      "boz",
      "cbr",
      "cba",
      "cbt",
      "cbz",
      "cb7",
      "vcd",
      "cfs",
      "chat",
      "pgn",
      "nsc",
      "cpio",
      "csh",
      "deb",
      "udeb",
      "dgc",
      "dir",
      "dcr",
      "dxr",
      "cst",
      "cct",
      "cxt",
      "w3d",
      "fgd",
      "swa",
      "wad",
      "ncx",
      "dtb",
      "res",
      "dvi",
      "evy",
      "eva",
      "bdf",
      "gsf",
      "psf",
      "otf",
      "pcf",
      "snf",
      "ttf",
      "ttc",
      "pfa",
      "pfb",
      "pfm",
      "afm",
      "arc",
      "spl",
      "gca",
      "ulx",
      "gnumeric",
      "gramps",
      "gtar",
      "hdf",
      "install",
      "iso",
      "jnlp",
      "latex",
      "lzh",
      "lha",
      "mie",
      "prc",
      "mobi",
      "application",
      "lnk",
      "wmd",
      "wmz",
      "xbap",
      "mdb",
      "obd",
      "crd",
      "clp",
      "exe",
      "dll",
      "com",
      "bat",
      "msi",
      "mvb",
      "m13",
      "m14",
      "wmf",
      "emf",
      "emz",
      "mny",
      "pub",
      "scd",
      "trm",
      "wri",
      "nc",
      "cdf",
      "nzb",
      "p12",
      "pfx",
      "p7b",
      "spc",
      "p7r",
      "rar",
      "ris",
      "sh",
      "shar",
      "swf",
      "xap",
      "sql",
      "sit",
      "sitx",
      "srt",
      "sv4cpio",
      "sv4crc",
      "t3",
      "gam",
      "tar",
      "tcl",
      "tex",
      "tfm",
      "texinfo",
      "texi",
      "obj",
      "ustar",
      "src",
      "der",
      "crt",
      "fig",
      "xlf",
      "xpi",
      "xz",
      "z1",
      "z2",
      "z3",
      "z4",
      "z5",
      "z6",
      "z7",
      "z8",
      "xaml",
      "xdf",
      "xenc",
      "xhtml",
      "xht",
      "xml",
      "xsl",
      "dtd",
      "xop",
      "xpl",
      "xslt",
      "xspf",
      "mxml",
      "xhvml",
      "xvml",
      "xvm",
      "yang",
      "yin",
      "zip",
      "adp",
      "au",
      "snd",
      "mid",
      "midi",
      "kar",
      "rmi",
      "m4a",
      "mp4a",
      "mpga",
      "mp2",
      "mp2a",
      "mp3",
      "m2a",
      "m3a",
      "oga",
      "ogg",
      "spx",
      "s3m",
      "sil",
      "uva",
      "uvva",
      "eol",
      "dra",
      "dts",
      "dtshd",
      "lvp",
      "pya",
      "ecelp4800",
      "ecelp7470",
      "ecelp9600",
      "rip",
      "weba",
      "aac",
      "aif",
      "aiff",
      "aifc",
      "caf",
      "flac",
      "mka",
      "m3u",
      "wax",
      "wma",
      "ram",
      "ra",
      "rmp",
      "wav",
      "xm",
      "cdx",
      "cif",
      "cmdf",
      "cml",
      "csml",
      "xyz",
      "bmp",
      "cgm",
      "g3",
      "gif",
      "ief",
      "jpeg",
      "jpg",
      "jpe",
      "ktx",
      "png",
      "btif",
      "sgi",
      "svg",
      "svgz",
      "tiff",
      "tif",
      "psd",
      "uvi",
      "uvvi",
      "uvg",
      "uvvg",
      "djvu",
      "djv",
      "sub",
      "dwg",
      "dxf",
      "fbs",
      "fpx",
      "fst",
      "mmr",
      "rlc",
      "mdi",
      "wdp",
      "npx",
      "wbmp",
      "xif",
      "webp",
      "3ds",
      "ras",
      "cmx",
      "fh",
      "fhc",
      "fh4",
      "fh5",
      "fh7",
      "ico",
      "sid",
      "pcx",
      "pic",
      "pct",
      "pnm",
      "pbm",
      "pgm",
      "ppm",
      "rgb",
      "tga",
      "xbm",
      "xpm",
      "xwd",
      "eml",
      "mime",
      "igs",
      "iges",
      "msh",
      "mesh",
      "silo",
      "dae",
      "dwf",
      "gdl",
      "gtw",
      "mts",
      "vtu",
      "wrl",
      "vrml",
      "x3db",
      "x3dbz",
      "x3dv",
      "x3dvz",
      "x3d",
      "x3dz",
      "appcache",
      "ics",
      "ifb",
      "css",
      "csv",
      "html",
      "htm",
      "n3",
      "txt",
      "text",
      "conf",
      "def",
      "list",
      "log",
      "in",
      "dsc",
      "rtx",
      "sgml",
      "sgm",
      "tsv",
      "t",
      "tr",
      "roff",
      "man",
      "me",
      "ms",
      "ttl",
      "uri",
      "uris",
      "urls",
      "vcard",
      "curl",
      "dcurl",
      "mcurl",
      "scurl",
      "fly",
      "flx",
      "gv",
      "3dml",
      "spot",
      "jad",
      "wml",
      "wmls",
      "s",
      "asm",
      "c",
      "cc",
      "cxx",
      "cpp",
      "h",
      "hh",
      "dic",
      "f",
      "for",
      "f77",
      "f90",
      "java",
      "nfo",
      "opml",
      "p",
      "pas",
      "etx",
      "sfv",
      "uu",
      "vcs",
      "vcf",
      "3gp",
      "3g2",
      "h261",
      "h263",
      "h264",
      "jpgv",
      "jpm",
      "jpgm",
      "mj2",
      "mjp2",
      "mp4",
      "mp4v",
      "mpg4",
      "mpeg",
      "mpg",
      "mpe",
      "m1v",
      "m2v",
      "ogv",
      "qt",
      "mov",
      "uvh",
      "uvvh",
      "uvm",
      "uvvm",
      "uvp",
      "uvvp",
      "uvs",
      "uvvs",
      "uvv",
      "uvvv",
      "dvb",
      "fvt",
      "mxu",
      "m4u",
      "pyv",
      "uvu",
      "uvvu",
      "viv",
      "webm",
      "f4v",
      "fli",
      "flv",
      "m4v",
      "mkv",
      "mk3d",
      "mks",
      "mng",
      "asf",
      "asx",
      "vob",
      "wm",
      "wmv",
      "wmx",
      "wvx",
      "avi",
      "movie",
      "smv",
      "ice",
      "map",
      "topojson",
      "jsonld",
      "geojson",
      "webmanifest",
      "webapp",
      "f4a",
      "f4b",
      "opus",
      "jng",
      "3gpp",
      "f4p",
      "cur",
      "woff2",
      "ear",
      "war",
      "img",
      "msm",
      "msp",
      "safariextz",
      "bbaw",
      "crx",
      "cco",
      "jardiff",
      "run",
      "oex",
      "pl",
      "pm",
      "rpm",
      "sea",
      "tk",
      "pem",
      "shtml",
      "mml",
      "xloc",
      "vtt",
      "htc"
    };
    public const string UnknownMimeType = "application/octet-stream";

    /// <summary>Dictionary of all available types</summary>
    public static ReadOnlyDictionary<string, string> TypeMap
    {
      get
      {
        return MimeTypes.LazyDict.Value;
      }
    }

    /// <param name="file">The raw file extensions (ex: "zip") or the file name or file path</param>
    /// <returns>The mime type string, returns "application/octet-stream" if no known type was found</returns>
    public static string GetMimeMapping(string file)
    {
      return MimeTypes.LookupType(!file.Contains(".") ? file : Path.GetExtension(file).Substring(1)) ?? "application/octet-stream";
    }

    private static string LookupType(string type)
    {
      switch (type)
      {
        case "123":
          return "application/vnd.lotus-1-2-3";
        case "3dml":
          return "text/vnd.in3d.3dml";
        case "3ds":
          return "image/x-3ds";
        case "3g2":
          return "video/3gpp2";
        case "3gp":
          return "video/3gpp";
        case "3gpp":
          return "video/3gpp";
        case "7z":
          return "application/x-7z-compressed";
        case "aab":
          return "application/x-authorware-bin";
        case "aac":
          return "audio/x-aac";
        case "aam":
          return "application/x-authorware-map";
        case "aas":
          return "application/x-authorware-seg";
        case "abw":
          return "application/x-abiword";
        case "ac":
          return "application/pkix-attr-cert";
        case "acc":
          return "application/vnd.americandynamics.acc";
        case "ace":
          return "application/x-ace-compressed";
        case "acu":
          return "application/vnd.acucobol";
        case "acutc":
          return "application/vnd.acucorp";
        case "adp":
          return "audio/adpcm";
        case "aep":
          return "application/vnd.audiograph";
        case "afm":
          return "application/x-font-type1";
        case "afp":
          return "application/vnd.ibm.modcap";
        case "ahead":
          return "application/vnd.ahead.space";
        case "ai":
          return "application/postscript";
        case "aif":
          return "audio/x-aiff";
        case "aifc":
          return "audio/x-aiff";
        case "aiff":
          return "audio/x-aiff";
        case "air":
          return "application/vnd.adobe.air-application-installer-package+zip";
        case "ait":
          return "application/vnd.dvb.ait";
        case "ami":
          return "application/vnd.amiga.ami";
        case "apk":
          return "application/vnd.android.package-archive";
        case "appcache":
          return "text/cache-manifest";
        case "application":
          return "application/x-ms-application";
        case "apr":
          return "application/vnd.lotus-approach";
        case "arc":
          return "application/x-freearc";
        case "asc":
          return "application/pgp-signature";
        case "asf":
          return "video/x-ms-asf";
        case "asm":
          return "text/x-asm";
        case "aso":
          return "application/vnd.accpac.simply.aso";
        case "asx":
          return "video/x-ms-asf";
        case "atc":
          return "application/vnd.acucorp";
        case "atom":
          return "application/atom+xml";
        case "atomcat":
          return "application/atomcat+xml";
        case "atomsvc":
          return "application/atomsvc+xml";
        case "atx":
          return "application/vnd.antix.game-component";
        case "au":
          return "audio/basic";
        case "avi":
          return "video/x-msvideo";
        case "aw":
          return "application/applixware";
        case "azf":
          return "application/vnd.airzip.filesecure.azf";
        case "azs":
          return "application/vnd.airzip.filesecure.azs";
        case "azw":
          return "application/vnd.amazon.ebook";
        case "bat":
          return "application/x-msdownload";
        case "bbaw":
          return "application/x-bb-appworld";
        case "bcpio":
          return "application/x-bcpio";
        case "bdf":
          return "application/x-font-bdf";
        case "bdm":
          return "application/vnd.syncml.dm+wbxml";
        case "bed":
          return "application/vnd.realvnc.bed";
        case "bh2":
          return "application/vnd.fujitsu.oasysprs";
        case "bin":
          return "application/octet-stream";
        case "blb":
          return "application/x-blorb";
        case "blorb":
          return "application/x-blorb";
        case "bmi":
          return "application/vnd.bmi";
        case "bmp":
          return "image/bmp";
        case "book":
          return "application/vnd.framemaker";
        case "box":
          return "application/vnd.previewsystems.box";
        case "boz":
          return "application/x-bzip2";
        case "bpk":
          return "application/octet-stream";
        case "btif":
          return "image/prs.btif";
        case "bz":
          return "application/x-bzip";
        case "bz2":
          return "application/x-bzip2";
        case "c":
          return "text/x-c";
        case "c11amc":
          return "application/vnd.cluetrust.cartomobile-config";
        case "c11amz":
          return "application/vnd.cluetrust.cartomobile-config-pkg";
        case "c4d":
          return "application/vnd.clonk.c4group";
        case "c4f":
          return "application/vnd.clonk.c4group";
        case "c4g":
          return "application/vnd.clonk.c4group";
        case "c4p":
          return "application/vnd.clonk.c4group";
        case "c4u":
          return "application/vnd.clonk.c4group";
        case "cab":
          return "application/vnd.ms-cab-compressed";
        case "caf":
          return "audio/x-caf";
        case "cap":
          return "application/vnd.tcpdump.pcap";
        case "car":
          return "application/vnd.curl.car";
        case "cat":
          return "application/vnd.ms-pki.seccat";
        case "cb7":
          return "application/x-cbr";
        case "cba":
          return "application/x-cbr";
        case "cbr":
          return "application/x-cbr";
        case "cbt":
          return "application/x-cbr";
        case "cbz":
          return "application/x-cbr";
        case "cc":
          return "text/x-c";
        case "cco":
          return "application/x-cocoa";
        case "cct":
          return "application/x-director";
        case "ccxml":
          return "application/ccxml+xml";
        case "cdbcmsg":
          return "application/vnd.contact.cmsg";
        case "cdf":
          return "application/x-netcdf";
        case "cdkey":
          return "application/vnd.mediastation.cdkey";
        case "cdmia":
          return "application/cdmi-capability";
        case "cdmic":
          return "application/cdmi-container";
        case "cdmid":
          return "application/cdmi-domain";
        case "cdmio":
          return "application/cdmi-object";
        case "cdmiq":
          return "application/cdmi-queue";
        case "cdx":
          return "chemical/x-cdx";
        case "cdxml":
          return "application/vnd.chemdraw+xml";
        case "cdy":
          return "application/vnd.cinderella";
        case "cer":
          return "application/pkix-cert";
        case "cfs":
          return "application/x-cfs-compressed";
        case "cgm":
          return "image/cgm";
        case "chat":
          return "application/x-chat";
        case "chm":
          return "application/vnd.ms-htmlhelp";
        case "chrt":
          return "application/vnd.kde.kchart";
        case "cif":
          return "chemical/x-cif";
        case "cii":
          return "application/vnd.anser-web-certificate-issue-initiation";
        case "cil":
          return "application/vnd.ms-artgalry";
        case "cla":
          return "application/vnd.claymore";
        case "class":
          return "application/java-vm";
        case "clkk":
          return "application/vnd.crick.clicker.keyboard";
        case "clkp":
          return "application/vnd.crick.clicker.palette";
        case "clkt":
          return "application/vnd.crick.clicker.template";
        case "clkw":
          return "application/vnd.crick.clicker.wordbank";
        case "clkx":
          return "application/vnd.crick.clicker";
        case "clp":
          return "application/x-msclip";
        case "cmc":
          return "application/vnd.cosmocaller";
        case "cmdf":
          return "chemical/x-cmdf";
        case "cml":
          return "chemical/x-cml";
        case "cmp":
          return "application/vnd.yellowriver-custom-menu";
        case "cmx":
          return "image/x-cmx";
        case "cod":
          return "application/vnd.rim.cod";
        case "com":
          return "application/x-msdownload";
        case "conf":
          return "text/plain";
        case "cpio":
          return "application/x-cpio";
        case "cpp":
          return "text/x-c";
        case "cpt":
          return "application/mac-compactpro";
        case "crd":
          return "application/x-mscardfile";
        case "crl":
          return "application/pkix-crl";
        case "crt":
          return "application/x-x509-ca-cert";
        case "crx":
          return "application/x-chrome-extension";
        case "cryptonote":
          return "application/vnd.rig.cryptonote";
        case "csh":
          return "application/x-csh";
        case "csml":
          return "chemical/x-csml";
        case "csp":
          return "application/vnd.commonspace";
        case "css":
          return "text/css";
        case "cst":
          return "application/x-director";
        case "csv":
          return "text/csv";
        case "cu":
          return "application/cu-seeme";
        case "cur":
          return "image/x-icon";
        case "curl":
          return "text/vnd.curl";
        case "cww":
          return "application/prs.cww";
        case "cxt":
          return "application/x-director";
        case "cxx":
          return "text/x-c";
        case "dae":
          return "model/vnd.collada+xml";
        case "daf":
          return "application/vnd.mobius.daf";
        case "dart":
          return "application/vnd.dart";
        case "dataless":
          return "application/vnd.fdsn.seed";
        case "davmount":
          return "application/davmount+xml";
        case "dbk":
          return "application/docbook+xml";
        case "dcr":
          return "application/x-director";
        case "dcurl":
          return "text/vnd.curl.dcurl";
        case "dd2":
          return "application/vnd.oma.dd2+xml";
        case "ddd":
          return "application/vnd.fujixerox.ddd";
        case "deb":
          return "application/x-debian-package";
        case "def":
          return "text/plain";
        case "deploy":
          return "application/octet-stream";
        case "der":
          return "application/x-x509-ca-cert";
        case "dfac":
          return "application/vnd.dreamfactory";
        case "dgc":
          return "application/x-dgc-compressed";
        case "dic":
          return "text/x-c";
        case "dir":
          return "application/x-director";
        case "dis":
          return "application/vnd.mobius.dis";
        case "dist":
          return "application/octet-stream";
        case "distz":
          return "application/octet-stream";
        case "djv":
          return "image/vnd.djvu";
        case "djvu":
          return "image/vnd.djvu";
        case "dll":
          return "application/x-msdownload";
        case "dmg":
          return "application/x-apple-diskimage";
        case "dmp":
          return "application/vnd.tcpdump.pcap";
        case "dms":
          return "application/octet-stream";
        case "dna":
          return "application/vnd.dna";
        case "doc":
          return "application/msword";
        case "docm":
          return "application/vnd.ms-word.document.macroenabled.12";
        case "docx":
          return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        case "dot":
          return "application/msword";
        case "dotm":
          return "application/vnd.ms-word.template.macroenabled.12";
        case "dotx":
          return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
        case "dp":
          return "application/vnd.osgi.dp";
        case "dpg":
          return "application/vnd.dpgraph";
        case "dra":
          return "audio/vnd.dra";
        case "dsc":
          return "text/prs.lines.tag";
        case "dssc":
          return "application/dssc+der";
        case "dtb":
          return "application/x-dtbook+xml";
        case "dtd":
          return "application/xml-dtd";
        case "dts":
          return "audio/vnd.dts";
        case "dtshd":
          return "audio/vnd.dts.hd";
        case "dump":
          return "application/octet-stream";
        case "dvb":
          return "video/vnd.dvb.file";
        case "dvi":
          return "application/x-dvi";
        case "dwf":
          return "model/vnd.dwf";
        case "dwg":
          return "image/vnd.dwg";
        case "dxf":
          return "image/vnd.dxf";
        case "dxp":
          return "application/vnd.spotfire.dxp";
        case "dxr":
          return "application/x-director";
        case "ear":
          return "application/java-archive";
        case "ecelp4800":
          return "audio/vnd.nuera.ecelp4800";
        case "ecelp7470":
          return "audio/vnd.nuera.ecelp7470";
        case "ecelp9600":
          return "audio/vnd.nuera.ecelp9600";
        case "ecma":
          return "application/ecmascript";
        case "edm":
          return "application/vnd.novadigm.edm";
        case "edx":
          return "application/vnd.novadigm.edx";
        case "efif":
          return "application/vnd.picsel";
        case "ei6":
          return "application/vnd.pg.osasli";
        case "elc":
          return "application/octet-stream";
        case "emf":
          return "application/x-msmetafile";
        case "eml":
          return "message/rfc822";
        case "emma":
          return "application/emma+xml";
        case "emz":
          return "application/x-msmetafile";
        case "eol":
          return "audio/vnd.digital-winds";
        case "eot":
          return "application/vnd.ms-fontobject";
        case "eps":
          return "application/postscript";
        case "epub":
          return "application/epub+zip";
        case "es3":
          return "application/vnd.eszigno3+xml";
        case "esa":
          return "application/vnd.osgi.subsystem";
        case "esf":
          return "application/vnd.epson.esf";
        case "et3":
          return "application/vnd.eszigno3+xml";
        case "etx":
          return "text/x-setext";
        case "eva":
          return "application/x-eva";
        case "evy":
          return "application/x-envoy";
        case "exe":
          return "application/x-msdownload";
        case "exi":
          return "application/exi";
        case "ext":
          return "application/vnd.novadigm.ext";
        case "ez":
          return "application/andrew-inset";
        case "ez2":
          return "application/vnd.ezpix-album";
        case "ez3":
          return "application/vnd.ezpix-package";
        case "f":
          return "text/x-fortran";
        case "f4a":
          return "audio/mp4";
        case "f4b":
          return "audio/mp4";
        case "f4p":
          return "video/mp4";
        case "f4v":
          return "video/x-f4v";
        case "f77":
          return "text/x-fortran";
        case "f90":
          return "text/x-fortran";
        case "fbs":
          return "image/vnd.fastbidsheet";
        case "fcdt":
          return "application/vnd.adobe.formscentral.fcdt";
        case "fcs":
          return "application/vnd.isac.fcs";
        case "fdf":
          return "application/vnd.fdf";
        case "fe_launch":
          return "application/vnd.denovo.fcselayout-link";
        case "fg5":
          return "application/vnd.fujitsu.oasysgp";
        case "fgd":
          return "application/x-director";
        case "fh":
          return "image/x-freehand";
        case "fh4":
          return "image/x-freehand";
        case "fh5":
          return "image/x-freehand";
        case "fh7":
          return "image/x-freehand";
        case "fhc":
          return "image/x-freehand";
        case "fig":
          return "application/x-xfig";
        case "flac":
          return "audio/x-flac";
        case "fli":
          return "video/x-fli";
        case "flo":
          return "application/vnd.micrografx.flo";
        case "flv":
          return "video/x-flv";
        case "flw":
          return "application/vnd.kde.kivio";
        case "flx":
          return "text/vnd.fmi.flexstor";
        case "fly":
          return "text/vnd.fly";
        case "fm":
          return "application/vnd.framemaker";
        case "fnc":
          return "application/vnd.frogans.fnc";
        case "for":
          return "text/x-fortran";
        case "fpx":
          return "image/vnd.fpx";
        case "frame":
          return "application/vnd.framemaker";
        case "fsc":
          return "application/vnd.fsc.weblaunch";
        case "fst":
          return "image/vnd.fst";
        case "ftc":
          return "application/vnd.fluxtime.clip";
        case "fti":
          return "application/vnd.anser-web-funds-transfer-initiation";
        case "fvt":
          return "video/vnd.fvt";
        case "fxp":
          return "application/vnd.adobe.fxp";
        case "fxpl":
          return "application/vnd.adobe.fxp";
        case "fzs":
          return "application/vnd.fuzzysheet";
        case "g2w":
          return "application/vnd.geoplan";
        case "g3":
          return "image/g3fax";
        case "g3w":
          return "application/vnd.geospace";
        case "gac":
          return "application/vnd.groove-account";
        case "gam":
          return "application/x-tads";
        case "gbr":
          return "application/rpki-ghostbusters";
        case "gca":
          return "application/x-gca-compressed";
        case "gdl":
          return "model/vnd.gdl";
        case "geo":
          return "application/vnd.dynageo";
        case "geojson":
          return "application/vnd.geo+json";
        case "gex":
          return "application/vnd.geometry-explorer";
        case "ggb":
          return "application/vnd.geogebra.file";
        case "ggt":
          return "application/vnd.geogebra.tool";
        case "ghf":
          return "application/vnd.groove-help";
        case "gif":
          return "image/gif";
        case "gim":
          return "application/vnd.groove-identity-message";
        case "gml":
          return "application/gml+xml";
        case "gmx":
          return "application/vnd.gmx";
        case "gnumeric":
          return "application/x-gnumeric";
        case "gph":
          return "application/vnd.flographit";
        case "gpx":
          return "application/gpx+xml";
        case "gqf":
          return "application/vnd.grafeq";
        case "gqs":
          return "application/vnd.grafeq";
        case "gram":
          return "application/srgs";
        case "gramps":
          return "application/x-gramps-xml";
        case "gre":
          return "application/vnd.geometry-explorer";
        case "grv":
          return "application/vnd.groove-injector";
        case "grxml":
          return "application/srgs+xml";
        case "gsf":
          return "application/x-font-ghostscript";
        case "gtar":
          return "application/x-gtar";
        case "gtm":
          return "application/vnd.groove-tool-message";
        case "gtw":
          return "model/vnd.gtw";
        case "gv":
          return "text/vnd.graphviz";
        case "gxf":
          return "application/gxf";
        case "gxt":
          return "application/vnd.geonext";
        case "h":
          return "text/x-c";
        case "h261":
          return "video/h261";
        case "h263":
          return "video/h263";
        case "h264":
          return "video/h264";
        case "hal":
          return "application/vnd.hal+xml";
        case "hbci":
          return "application/vnd.hbci";
        case "hdf":
          return "application/x-hdf";
        case "hh":
          return "text/x-c";
        case "hlp":
          return "application/winhlp";
        case "hpgl":
          return "application/vnd.hp-hpgl";
        case "hpid":
          return "application/vnd.hp-hpid";
        case "hps":
          return "application/vnd.hp-hps";
        case "hqx":
          return "application/mac-binhex40";
        case "htc":
          return "text/x-component";
        case "htke":
          return "application/vnd.kenameaapp";
        case "htm":
          return "text/html";
        case "html":
          return "text/html";
        case "hvd":
          return "application/vnd.yamaha.hv-dic";
        case "hvp":
          return "application/vnd.yamaha.hv-voice";
        case "hvs":
          return "application/vnd.yamaha.hv-script";
        case "i2g":
          return "application/vnd.intergeo";
        case "icc":
          return "application/vnd.iccprofile";
        case "ice":
          return "x-conference/x-cooltalk";
        case "icm":
          return "application/vnd.iccprofile";
        case "ico":
          return "image/x-icon";
        case "ics":
          return "text/calendar";
        case "ief":
          return "image/ief";
        case "ifb":
          return "text/calendar";
        case "ifm":
          return "application/vnd.shana.informed.formdata";
        case "iges":
          return "model/iges";
        case "igl":
          return "application/vnd.igloader";
        case "igm":
          return "application/vnd.insors.igm";
        case "igs":
          return "model/iges";
        case "igx":
          return "application/vnd.micrografx.igx";
        case "iif":
          return "application/vnd.shana.informed.interchange";
        case "img":
          return "application/octet-stream";
        case "imp":
          return "application/vnd.accpac.simply.imp";
        case "ims":
          return "application/vnd.ms-ims";
        case "in":
          return "text/plain";
        case "ink":
          return "application/inkml+xml";
        case "inkml":
          return "application/inkml+xml";
        case "install":
          return "application/x-install-instructions";
        case "iota":
          return "application/vnd.astraea-software.iota";
        case "ipfix":
          return "application/ipfix";
        case "ipk":
          return "application/vnd.shana.informed.package";
        case "irm":
          return "application/vnd.ibm.rights-management";
        case "irp":
          return "application/vnd.irepository.package+xml";
        case "iso":
          return "application/x-iso9660-image";
        case "itp":
          return "application/vnd.shana.informed.formtemplate";
        case "ivp":
          return "application/vnd.immervision-ivp";
        case "ivu":
          return "application/vnd.immervision-ivu";
        case "jad":
          return "text/vnd.sun.j2me.app-descriptor";
        case "jam":
          return "application/vnd.jam";
        case "jar":
          return "application/java-archive";
        case "jardiff":
          return "application/x-java-archive-diff";
        case "java":
          return "text/x-java-source";
        case "jisp":
          return "application/vnd.jisp";
        case "jlt":
          return "application/vnd.hp-jlyt";
        case "jng":
          return "image/x-jng";
        case "jnlp":
          return "application/x-java-jnlp-file";
        case "joda":
          return "application/vnd.joost.joda-archive";
        case "jpe":
          return "image/jpeg";
        case "jpeg":
          return "image/jpeg";
        case "jpg":
          return "image/jpeg";
        case "jpgm":
          return "video/jpm";
        case "jpgv":
          return "video/jpeg";
        case "jpm":
          return "video/jpm";
        case "js":
          return "application/javascript";
        case "json":
          return "application/json";
        case "jsonld":
          return "application/ld+json";
        case "jsonml":
          return "application/jsonml+json";
        case "kar":
          return "audio/midi";
        case "karbon":
          return "application/vnd.kde.karbon";
        case "kfo":
          return "application/vnd.kde.kformula";
        case "kia":
          return "application/vnd.kidspiration";
        case "kml":
          return "application/vnd.google-earth.kml+xml";
        case "kmz":
          return "application/vnd.google-earth.kmz";
        case "kne":
          return "application/vnd.kinar";
        case "knp":
          return "application/vnd.kinar";
        case "kon":
          return "application/vnd.kde.kontour";
        case "kpr":
          return "application/vnd.kde.kpresenter";
        case "kpt":
          return "application/vnd.kde.kpresenter";
        case "kpxx":
          return "application/vnd.ds-keypoint";
        case "ksp":
          return "application/vnd.kde.kspread";
        case "ktr":
          return "application/vnd.kahootz";
        case "ktx":
          return "image/ktx";
        case "ktz":
          return "application/vnd.kahootz";
        case "kwd":
          return "application/vnd.kde.kword";
        case "kwt":
          return "application/vnd.kde.kword";
        case "lasxml":
          return "application/vnd.las.las+xml";
        case "latex":
          return "application/x-latex";
        case "lbd":
          return "application/vnd.llamagraphics.life-balance.desktop";
        case "lbe":
          return "application/vnd.llamagraphics.life-balance.exchange+xml";
        case "les":
          return "application/vnd.hhe.lesson-player";
        case "lha":
          return "application/x-lzh-compressed";
        case "link66":
          return "application/vnd.route66.link66+xml";
        case "list":
          return "text/plain";
        case "list3820":
          return "application/vnd.ibm.modcap";
        case "listafp":
          return "application/vnd.ibm.modcap";
        case "lnk":
          return "application/x-ms-shortcut";
        case "log":
          return "text/plain";
        case "lostxml":
          return "application/lost+xml";
        case "lrf":
          return "application/octet-stream";
        case "lrm":
          return "application/vnd.ms-lrm";
        case "ltf":
          return "application/vnd.frogans.ltf";
        case "lvp":
          return "audio/vnd.lucent.voice";
        case "lwp":
          return "application/vnd.lotus-wordpro";
        case "lzh":
          return "application/x-lzh-compressed";
        case "m13":
          return "application/x-msmediaview";
        case "m14":
          return "application/x-msmediaview";
        case "m1v":
          return "video/mpeg";
        case "m21":
          return "application/mp21";
        case "m2a":
          return "audio/mpeg";
        case "m2v":
          return "video/mpeg";
        case "m3a":
          return "audio/mpeg";
        case "m3u":
          return "audio/x-mpegurl";
        case "m3u8":
          return "application/vnd.apple.mpegurl";
        case "m4a":
          return "audio/mp4";
        case "m4u":
          return "video/vnd.mpegurl";
        case "m4v":
          return "video/x-m4v";
        case "ma":
          return "application/mathematica";
        case "mads":
          return "application/mads+xml";
        case "mag":
          return "application/vnd.ecowin.chart";
        case "maker":
          return "application/vnd.framemaker";
        case "man":
          return "text/troff";
        case "map":
          return "application/json";
        case "mar":
          return "application/octet-stream";
        case "mathml":
          return "application/mathml+xml";
        case "mb":
          return "application/mathematica";
        case "mbk":
          return "application/vnd.mobius.mbk";
        case "mbox":
          return "application/mbox";
        case "mc1":
          return "application/vnd.medcalcdata";
        case "mcd":
          return "application/vnd.mcd";
        case "mcurl":
          return "text/vnd.curl.mcurl";
        case "mdb":
          return "application/x-msaccess";
        case "mdi":
          return "image/vnd.ms-modi";
        case "me":
          return "text/troff";
        case "mesh":
          return "model/mesh";
        case "meta4":
          return "application/metalink4+xml";
        case "metalink":
          return "application/metalink+xml";
        case "mets":
          return "application/mets+xml";
        case "mfm":
          return "application/vnd.mfmp";
        case "mft":
          return "application/rpki-manifest";
        case "mgp":
          return "application/vnd.osgeo.mapguide.package";
        case "mgz":
          return "application/vnd.proteus.magazine";
        case "mid":
          return "audio/midi";
        case "midi":
          return "audio/midi";
        case "mie":
          return "application/x-mie";
        case "mif":
          return "application/vnd.mif";
        case "mime":
          return "message/rfc822";
        case "mj2":
          return "video/mj2";
        case "mjp2":
          return "video/mj2";
        case "mk3d":
          return "video/x-matroska";
        case "mka":
          return "audio/x-matroska";
        case "mks":
          return "video/x-matroska";
        case "mkv":
          return "video/x-matroska";
        case "mlp":
          return "application/vnd.dolby.mlp";
        case "mmd":
          return "application/vnd.chipnuts.karaoke-mmd";
        case "mmf":
          return "application/vnd.smaf";
        case "mml":
          return "text/mathml";
        case "mmr":
          return "image/vnd.fujixerox.edmics-mmr";
        case "mng":
          return "video/x-mng";
        case "mny":
          return "application/x-msmoney";
        case "mobi":
          return "application/x-mobipocket-ebook";
        case "mods":
          return "application/mods+xml";
        case "mov":
          return "video/quicktime";
        case "movie":
          return "video/x-sgi-movie";
        case "mp2":
          return "audio/mpeg";
        case "mp21":
          return "application/mp21";
        case "mp2a":
          return "audio/mpeg";
        case "mp3":
          return "audio/mpeg";
        case "mp4":
          return "video/mp4";
        case "mp4a":
          return "audio/mp4";
        case "mp4s":
          return "application/mp4";
        case "mp4v":
          return "video/mp4";
        case "mpc":
          return "application/vnd.mophun.certificate";
        case "mpe":
          return "video/mpeg";
        case "mpeg":
          return "video/mpeg";
        case "mpg":
          return "video/mpeg";
        case "mpg4":
          return "video/mp4";
        case "mpga":
          return "audio/mpeg";
        case "mpkg":
          return "application/vnd.apple.installer+xml";
        case "mpm":
          return "application/vnd.blueice.multipass";
        case "mpn":
          return "application/vnd.mophun.application";
        case "mpp":
          return "application/vnd.ms-project";
        case "mpt":
          return "application/vnd.ms-project";
        case "mpy":
          return "application/vnd.ibm.minipay";
        case "mqy":
          return "application/vnd.mobius.mqy";
        case "mrc":
          return "application/marc";
        case "mrcx":
          return "application/marcxml+xml";
        case "ms":
          return "text/troff";
        case "mscml":
          return "application/mediaservercontrol+xml";
        case "mseed":
          return "application/vnd.fdsn.mseed";
        case "mseq":
          return "application/vnd.mseq";
        case "msf":
          return "application/vnd.epson.msf";
        case "msh":
          return "model/mesh";
        case "msi":
          return "application/x-msdownload";
        case "msl":
          return "application/vnd.mobius.msl";
        case "msm":
          return "application/octet-stream";
        case "msp":
          return "application/octet-stream";
        case "msty":
          return "application/vnd.muvee.style";
        case "mts":
          return "model/vnd.mts";
        case "mus":
          return "application/vnd.musician";
        case "musicxml":
          return "application/vnd.recordare.musicxml+xml";
        case "mvb":
          return "application/x-msmediaview";
        case "mwf":
          return "application/vnd.mfer";
        case "mxf":
          return "application/mxf";
        case "mxl":
          return "application/vnd.recordare.musicxml";
        case "mxml":
          return "application/xv+xml";
        case "mxs":
          return "application/vnd.triscape.mxs";
        case "mxu":
          return "video/vnd.mpegurl";
        case "n-gage":
          return "application/vnd.nokia.n-gage.symbian.install";
        case "n3":
          return "text/n3";
        case "nb":
          return "application/mathematica";
        case "nbp":
          return "application/vnd.wolfram.player";
        case "nc":
          return "application/x-netcdf";
        case "ncx":
          return "application/x-dtbncx+xml";
        case "nfo":
          return "text/x-nfo";
        case "ngdat":
          return "application/vnd.nokia.n-gage.data";
        case "nitf":
          return "application/vnd.nitf";
        case "nlu":
          return "application/vnd.neurolanguage.nlu";
        case "nml":
          return "application/vnd.enliven";
        case "nnd":
          return "application/vnd.noblenet-directory";
        case "nns":
          return "application/vnd.noblenet-sealer";
        case "nnw":
          return "application/vnd.noblenet-web";
        case "npx":
          return "image/vnd.net-fpx";
        case "nsc":
          return "application/x-conference";
        case "nsf":
          return "application/vnd.lotus-notes";
        case "ntf":
          return "application/vnd.nitf";
        case "nzb":
          return "application/x-nzb";
        case "oa2":
          return "application/vnd.fujitsu.oasys2";
        case "oa3":
          return "application/vnd.fujitsu.oasys3";
        case "oas":
          return "application/vnd.fujitsu.oasys";
        case "obd":
          return "application/x-msbinder";
        case "obj":
          return "application/x-tgif";
        case "oda":
          return "application/oda";
        case "odb":
          return "application/vnd.oasis.opendocument.database";
        case "odc":
          return "application/vnd.oasis.opendocument.chart";
        case "odf":
          return "application/vnd.oasis.opendocument.formula";
        case "odft":
          return "application/vnd.oasis.opendocument.formula-template";
        case "odg":
          return "application/vnd.oasis.opendocument.graphics";
        case "odi":
          return "application/vnd.oasis.opendocument.image";
        case "odm":
          return "application/vnd.oasis.opendocument.text-master";
        case "odp":
          return "application/vnd.oasis.opendocument.presentation";
        case "ods":
          return "application/vnd.oasis.opendocument.spreadsheet";
        case "odt":
          return "application/vnd.oasis.opendocument.text";
        case "oex":
          return "application/x-opera-extension";
        case "oga":
          return "audio/ogg";
        case "ogg":
          return "audio/ogg";
        case "ogv":
          return "video/ogg";
        case "ogx":
          return "application/ogg";
        case "omdoc":
          return "application/omdoc+xml";
        case "onepkg":
          return "application/onenote";
        case "onetmp":
          return "application/onenote";
        case "onetoc":
          return "application/onenote";
        case "onetoc2":
          return "application/onenote";
        case "opf":
          return "application/oebps-package+xml";
        case "opml":
          return "text/x-opml";
        case "oprc":
          return "application/vnd.palm";
        case "opus":
          return "audio/ogg";
        case "org":
          return "application/vnd.lotus-organizer";
        case "osf":
          return "application/vnd.yamaha.openscoreformat";
        case "osfpvg":
          return "application/vnd.yamaha.openscoreformat.osfpvg+xml";
        case "otc":
          return "application/vnd.oasis.opendocument.chart-template";
        case "otf":
          return "application/x-font-otf";
        case "otg":
          return "application/vnd.oasis.opendocument.graphics-template";
        case "oth":
          return "application/vnd.oasis.opendocument.text-web";
        case "oti":
          return "application/vnd.oasis.opendocument.image-template";
        case "otp":
          return "application/vnd.oasis.opendocument.presentation-template";
        case "ots":
          return "application/vnd.oasis.opendocument.spreadsheet-template";
        case "ott":
          return "application/vnd.oasis.opendocument.text-template";
        case "oxps":
          return "application/oxps";
        case "oxt":
          return "application/vnd.openofficeorg.extension";
        case "p":
          return "text/x-pascal";
        case "p10":
          return "application/pkcs10";
        case "p12":
          return "application/x-pkcs12";
        case "p7b":
          return "application/x-pkcs7-certificates";
        case "p7c":
          return "application/pkcs7-mime";
        case "p7m":
          return "application/pkcs7-mime";
        case "p7r":
          return "application/x-pkcs7-certreqresp";
        case "p7s":
          return "application/pkcs7-signature";
        case "p8":
          return "application/pkcs8";
        case "pas":
          return "text/x-pascal";
        case "paw":
          return "application/vnd.pawaafile";
        case "pbd":
          return "application/vnd.powerbuilder6";
        case "pbm":
          return "image/x-portable-bitmap";
        case "pcap":
          return "application/vnd.tcpdump.pcap";
        case "pcf":
          return "application/x-font-pcf";
        case "pcl":
          return "application/vnd.hp-pcl";
        case "pclxl":
          return "application/vnd.hp-pclxl";
        case "pct":
          return "image/x-pict";
        case "pcurl":
          return "application/vnd.curl.pcurl";
        case "pcx":
          return "image/x-pcx";
        case "pdb":
          return "application/vnd.palm";
        case "pdf":
          return "application/pdf";
        case "pem":
          return "application/x-x509-ca-cert";
        case "pfa":
          return "application/x-font-type1";
        case "pfb":
          return "application/x-font-type1";
        case "pfm":
          return "application/x-font-type1";
        case "pfr":
          return "application/font-tdpfr";
        case "pfx":
          return "application/x-pkcs12";
        case "pgm":
          return "image/x-portable-graymap";
        case "pgn":
          return "application/x-chess-pgn";
        case "pgp":
          return "application/pgp-encrypted";
        case "pic":
          return "image/x-pict";
        case "pkg":
          return "application/octet-stream";
        case "pki":
          return "application/pkixcmp";
        case "pkipath":
          return "application/pkix-pkipath";
        case "pl":
          return "application/x-perl";
        case "plb":
          return "application/vnd.3gpp.pic-bw-large";
        case "plc":
          return "application/vnd.mobius.plc";
        case "plf":
          return "application/vnd.pocketlearn";
        case "pls":
          return "application/pls+xml";
        case "pm":
          return "application/x-perl";
        case "pml":
          return "application/vnd.ctc-posml";
        case "png":
          return "image/png";
        case "pnm":
          return "image/x-portable-anymap";
        case "portpkg":
          return "application/vnd.macports.portpkg";
        case "pot":
          return "application/vnd.ms-powerpoint";
        case "potm":
          return "application/vnd.ms-powerpoint.template.macroenabled.12";
        case "potx":
          return "application/vnd.openxmlformats-officedocument.presentationml.template";
        case "ppam":
          return "application/vnd.ms-powerpoint.addin.macroenabled.12";
        case "ppd":
          return "application/vnd.cups-ppd";
        case "ppm":
          return "image/x-portable-pixmap";
        case "pps":
          return "application/vnd.ms-powerpoint";
        case "ppsm":
          return "application/vnd.ms-powerpoint.slideshow.macroenabled.12";
        case "ppsx":
          return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
        case "ppt":
          return "application/vnd.ms-powerpoint";
        case "pptm":
          return "application/vnd.ms-powerpoint.presentation.macroenabled.12";
        case "pptx":
          return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        case "pqa":
          return "application/vnd.palm";
        case "prc":
          return "application/x-mobipocket-ebook";
        case "pre":
          return "application/vnd.lotus-freelance";
        case "prf":
          return "application/pics-rules";
        case "ps":
          return "application/postscript";
        case "psb":
          return "application/vnd.3gpp.pic-bw-small";
        case "psd":
          return "image/vnd.adobe.photoshop";
        case "psf":
          return "application/x-font-linux-psf";
        case "pskcxml":
          return "application/pskc+xml";
        case "ptid":
          return "application/vnd.pvi.ptid1";
        case "pub":
          return "application/x-mspublisher";
        case "pvb":
          return "application/vnd.3gpp.pic-bw-var";
        case "pwn":
          return "application/vnd.3m.post-it-notes";
        case "pya":
          return "audio/vnd.ms-playready.media.pya";
        case "pyv":
          return "video/vnd.ms-playready.media.pyv";
        case "qam":
          return "application/vnd.epson.quickanime";
        case "qbo":
          return "application/vnd.intu.qbo";
        case "qfx":
          return "application/vnd.intu.qfx";
        case "qps":
          return "application/vnd.publishare-delta-tree";
        case "qt":
          return "video/quicktime";
        case "qwd":
          return "application/vnd.quark.quarkxpress";
        case "qwt":
          return "application/vnd.quark.quarkxpress";
        case "qxb":
          return "application/vnd.quark.quarkxpress";
        case "qxd":
          return "application/vnd.quark.quarkxpress";
        case "qxl":
          return "application/vnd.quark.quarkxpress";
        case "qxt":
          return "application/vnd.quark.quarkxpress";
        case "ra":
          return "audio/x-pn-realaudio";
        case "ram":
          return "audio/x-pn-realaudio";
        case "rar":
          return "application/x-rar-compressed";
        case "ras":
          return "image/x-cmu-raster";
        case "rcprofile":
          return "application/vnd.ipunplugged.rcprofile";
        case "rdf":
          return "application/rdf+xml";
        case "rdz":
          return "application/vnd.data-vision.rdz";
        case "rep":
          return "application/vnd.businessobjects";
        case "res":
          return "application/x-dtbresource+xml";
        case "rgb":
          return "image/x-rgb";
        case "rif":
          return "application/reginfo+xml";
        case "rip":
          return "audio/vnd.rip";
        case "ris":
          return "application/x-research-info-systems";
        case "rl":
          return "application/resource-lists+xml";
        case "rlc":
          return "image/vnd.fujixerox.edmics-rlc";
        case "rld":
          return "application/resource-lists-diff+xml";
        case "rm":
          return "application/vnd.rn-realmedia";
        case "rmi":
          return "audio/midi";
        case "rmp":
          return "audio/x-pn-realaudio-plugin";
        case "rms":
          return "application/vnd.jcp.javame.midlet-rms";
        case "rmvb":
          return "application/vnd.rn-realmedia-vbr";
        case "rnc":
          return "application/relax-ng-compact-syntax";
        case "roa":
          return "application/rpki-roa";
        case "roff":
          return "text/troff";
        case "rp9":
          return "application/vnd.cloanto.rp9";
        case "rpm":
          return "application/x-redhat-package-manager";
        case "rpss":
          return "application/vnd.nokia.radio-presets";
        case "rpst":
          return "application/vnd.nokia.radio-preset";
        case "rq":
          return "application/sparql-query";
        case "rs":
          return "application/rls-services+xml";
        case "rsd":
          return "application/rsd+xml";
        case "rss":
          return "application/rss+xml";
        case "rtf":
          return "application/rtf";
        case "rtx":
          return "text/richtext";
        case "run":
          return "application/x-makeself";
        case "s":
          return "text/x-asm";
        case "s3m":
          return "audio/s3m";
        case "saf":
          return "application/vnd.yamaha.smaf-audio";
        case "safariextz":
          return "application/octet-stream";
        case "sbml":
          return "application/sbml+xml";
        case "sc":
          return "application/vnd.ibm.secure-container";
        case "scd":
          return "application/x-msschedule";
        case "scm":
          return "application/vnd.lotus-screencam";
        case "scq":
          return "application/scvp-cv-request";
        case "scs":
          return "application/scvp-cv-response";
        case "scurl":
          return "text/vnd.curl.scurl";
        case "sda":
          return "application/vnd.stardivision.draw";
        case "sdc":
          return "application/vnd.stardivision.calc";
        case "sdd":
          return "application/vnd.stardivision.impress";
        case "sdkd":
          return "application/vnd.solent.sdkm+xml";
        case "sdkm":
          return "application/vnd.solent.sdkm+xml";
        case "sdp":
          return "application/sdp";
        case "sdw":
          return "application/vnd.stardivision.writer";
        case "sea":
          return "application/x-sea";
        case "see":
          return "application/vnd.seemail";
        case "seed":
          return "application/vnd.fdsn.seed";
        case "sema":
          return "application/vnd.sema";
        case "semd":
          return "application/vnd.semd";
        case "semf":
          return "application/vnd.semf";
        case "ser":
          return "application/java-serialized-object";
        case "setpay":
          return "application/set-payment-initiation";
        case "setreg":
          return "application/set-registration-initiation";
        case "sfd-hdstx":
          return "application/vnd.hydrostatix.sof-data";
        case "sfs":
          return "application/vnd.spotfire.sfs";
        case "sfv":
          return "text/x-sfv";
        case "sgi":
          return "image/sgi";
        case "sgl":
          return "application/vnd.stardivision.writer-global";
        case "sgm":
          return "text/sgml";
        case "sgml":
          return "text/sgml";
        case "sh":
          return "application/x-sh";
        case "shar":
          return "application/x-shar";
        case "shf":
          return "application/shf+xml";
        case "shtml":
          return "text/html";
        case "sid":
          return "image/x-mrsid-image";
        case "sig":
          return "application/pgp-signature";
        case "sil":
          return "audio/silk";
        case "silo":
          return "model/mesh";
        case "sis":
          return "application/vnd.symbian.install";
        case "sisx":
          return "application/vnd.symbian.install";
        case "sit":
          return "application/x-stuffit";
        case "sitx":
          return "application/x-stuffitx";
        case "skd":
          return "application/vnd.koan";
        case "skm":
          return "application/vnd.koan";
        case "skp":
          return "application/vnd.koan";
        case "skt":
          return "application/vnd.koan";
        case "sldm":
          return "application/vnd.ms-powerpoint.slide.macroenabled.12";
        case "sldx":
          return "application/vnd.openxmlformats-officedocument.presentationml.slide";
        case "slt":
          return "application/vnd.epson.salt";
        case "sm":
          return "application/vnd.stepmania.stepchart";
        case "smf":
          return "application/vnd.stardivision.math";
        case "smi":
          return "application/smil+xml";
        case "smil":
          return "application/smil+xml";
        case "smv":
          return "video/x-smv";
        case "smzip":
          return "application/vnd.stepmania.package";
        case "snd":
          return "audio/basic";
        case "snf":
          return "application/x-font-snf";
        case "so":
          return "application/octet-stream";
        case "spc":
          return "application/x-pkcs7-certificates";
        case "spf":
          return "application/vnd.yamaha.smaf-phrase";
        case "spl":
          return "application/x-futuresplash";
        case "spot":
          return "text/vnd.in3d.spot";
        case "spp":
          return "application/scvp-vp-response";
        case "spq":
          return "application/scvp-vp-request";
        case "spx":
          return "audio/ogg";
        case "sql":
          return "application/x-sql";
        case "src":
          return "application/x-wais-source";
        case "srt":
          return "application/x-subrip";
        case "sru":
          return "application/sru+xml";
        case "srx":
          return "application/sparql-results+xml";
        case "ssdl":
          return "application/ssdl+xml";
        case "sse":
          return "application/vnd.kodak-descriptor";
        case "ssf":
          return "application/vnd.epson.ssf";
        case "ssml":
          return "application/ssml+xml";
        case "st":
          return "application/vnd.sailingtracker.track";
        case "stc":
          return "application/vnd.sun.xml.calc.template";
        case "std":
          return "application/vnd.sun.xml.draw.template";
        case "stf":
          return "application/vnd.wt.stf";
        case "sti":
          return "application/vnd.sun.xml.impress.template";
        case "stk":
          return "application/hyperstudio";
        case "stl":
          return "application/vnd.ms-pki.stl";
        case "str":
          return "application/vnd.pg.format";
        case "stw":
          return "application/vnd.sun.xml.writer.template";
        case "sub":
          return "image/vnd.dvb.subtitle";
        case "sus":
          return "application/vnd.sus-calendar";
        case "susp":
          return "application/vnd.sus-calendar";
        case "sv4cpio":
          return "application/x-sv4cpio";
        case "sv4crc":
          return "application/x-sv4crc";
        case "svc":
          return "application/vnd.dvb.service";
        case "svd":
          return "application/vnd.svd";
        case "svg":
          return "image/svg+xml";
        case "svgz":
          return "image/svg+xml";
        case "swa":
          return "application/x-director";
        case "swf":
          return "application/x-shockwave-flash";
        case "swi":
          return "application/vnd.aristanetworks.swi";
        case "sxc":
          return "application/vnd.sun.xml.calc";
        case "sxd":
          return "application/vnd.sun.xml.draw";
        case "sxg":
          return "application/vnd.sun.xml.writer.global";
        case "sxi":
          return "application/vnd.sun.xml.impress";
        case "sxm":
          return "application/vnd.sun.xml.math";
        case "sxw":
          return "application/vnd.sun.xml.writer";
        case "t":
          return "text/troff";
        case "t3":
          return "application/x-t3vm-image";
        case "taglet":
          return "application/vnd.mynfc";
        case "tao":
          return "application/vnd.tao.intent-module-archive";
        case "tar":
          return "application/x-tar";
        case "tcap":
          return "application/vnd.3gpp2.tcap";
        case "tcl":
          return "application/x-tcl";
        case "teacher":
          return "application/vnd.smart.teacher";
        case "tei":
          return "application/tei+xml";
        case "teicorpus":
          return "application/tei+xml";
        case "tex":
          return "application/x-tex";
        case "texi":
          return "application/x-texinfo";
        case "texinfo":
          return "application/x-texinfo";
        case "text":
          return "text/plain";
        case "tfi":
          return "application/thraud+xml";
        case "tfm":
          return "application/x-tex-tfm";
        case "tga":
          return "image/x-tga";
        case "thmx":
          return "application/vnd.ms-officetheme";
        case "tif":
          return "image/tiff";
        case "tiff":
          return "image/tiff";
        case "tk":
          return "application/x-tcl";
        case "tmo":
          return "application/vnd.tmobile-livetv";
        case "topojson":
          return "application/json";
        case "torrent":
          return "application/x-bittorrent";
        case "tpl":
          return "application/vnd.groove-tool-template";
        case "tpt":
          return "application/vnd.trid.tpt";
        case "tr":
          return "text/troff";
        case "tra":
          return "application/vnd.trueapp";
        case "trm":
          return "application/x-msterminal";
        case "tsd":
          return "application/timestamped-data";
        case "tsv":
          return "text/tab-separated-values";
        case "ttc":
          return "application/x-font-ttf";
        case "ttf":
          return "application/x-font-ttf";
        case "ttl":
          return "text/turtle";
        case "twd":
          return "application/vnd.simtech-mindmapper";
        case "twds":
          return "application/vnd.simtech-mindmapper";
        case "txd":
          return "application/vnd.genomatix.tuxedo";
        case "txf":
          return "application/vnd.mobius.txf";
        case "txt":
          return "text/plain";
        case "u32":
          return "application/x-authorware-bin";
        case "udeb":
          return "application/x-debian-package";
        case "ufd":
          return "application/vnd.ufdl";
        case "ufdl":
          return "application/vnd.ufdl";
        case "ulx":
          return "application/x-glulx";
        case "umj":
          return "application/vnd.umajin";
        case "unityweb":
          return "application/vnd.unity";
        case "uoml":
          return "application/vnd.uoml+xml";
        case "uri":
          return "text/uri-list";
        case "uris":
          return "text/uri-list";
        case "urls":
          return "text/uri-list";
        case "ustar":
          return "application/x-ustar";
        case "utz":
          return "application/vnd.uiq.theme";
        case "uu":
          return "text/x-uuencode";
        case "uva":
          return "audio/vnd.dece.audio";
        case "uvd":
          return "application/vnd.dece.data";
        case "uvf":
          return "application/vnd.dece.data";
        case "uvg":
          return "image/vnd.dece.graphic";
        case "uvh":
          return "video/vnd.dece.hd";
        case "uvi":
          return "image/vnd.dece.graphic";
        case "uvm":
          return "video/vnd.dece.mobile";
        case "uvp":
          return "video/vnd.dece.pd";
        case "uvs":
          return "video/vnd.dece.sd";
        case "uvt":
          return "application/vnd.dece.ttml+xml";
        case "uvu":
          return "video/vnd.uvvu.mp4";
        case "uvv":
          return "video/vnd.dece.video";
        case "uvva":
          return "audio/vnd.dece.audio";
        case "uvvd":
          return "application/vnd.dece.data";
        case "uvvf":
          return "application/vnd.dece.data";
        case "uvvg":
          return "image/vnd.dece.graphic";
        case "uvvh":
          return "video/vnd.dece.hd";
        case "uvvi":
          return "image/vnd.dece.graphic";
        case "uvvm":
          return "video/vnd.dece.mobile";
        case "uvvp":
          return "video/vnd.dece.pd";
        case "uvvs":
          return "video/vnd.dece.sd";
        case "uvvt":
          return "application/vnd.dece.ttml+xml";
        case "uvvu":
          return "video/vnd.uvvu.mp4";
        case "uvvv":
          return "video/vnd.dece.video";
        case "uvvx":
          return "application/vnd.dece.unspecified";
        case "uvvz":
          return "application/vnd.dece.zip";
        case "uvx":
          return "application/vnd.dece.unspecified";
        case "uvz":
          return "application/vnd.dece.zip";
        case "vcard":
          return "text/vcard";
        case "vcd":
          return "application/x-cdlink";
        case "vcf":
          return "text/x-vcard";
        case "vcg":
          return "application/vnd.groove-vcard";
        case "vcs":
          return "text/x-vcalendar";
        case "vcx":
          return "application/vnd.vcx";
        case "vis":
          return "application/vnd.visionary";
        case "viv":
          return "video/vnd.vivo";
        case "vob":
          return "video/x-ms-vob";
        case "vor":
          return "application/vnd.stardivision.writer";
        case "vox":
          return "application/x-authorware-bin";
        case "vrml":
          return "model/vrml";
        case "vsd":
          return "application/vnd.visio";
        case "vsf":
          return "application/vnd.vsf";
        case "vss":
          return "application/vnd.visio";
        case "vst":
          return "application/vnd.visio";
        case "vsw":
          return "application/vnd.visio";
        case "vtt":
          return "text/vtt";
        case "vtu":
          return "model/vnd.vtu";
        case "vxml":
          return "application/voicexml+xml";
        case "w3d":
          return "application/x-director";
        case "wad":
          return "application/x-doom";
        case "war":
          return "application/java-archive";
        case "wav":
          return "audio/x-wav";
        case "wax":
          return "audio/x-ms-wax";
        case "wbmp":
          return "image/vnd.wap.wbmp";
        case "wbs":
          return "application/vnd.criticaltools.wbs+xml";
        case "wbxml":
          return "application/vnd.wap.wbxml";
        case "wcm":
          return "application/vnd.ms-works";
        case "wdb":
          return "application/vnd.ms-works";
        case "wdp":
          return "image/vnd.ms-photo";
        case "weba":
          return "audio/webm";
        case "webapp":
          return "application/x-web-app-manifest+json";
        case "webm":
          return "video/webm";
        case "webmanifest":
          return "application/manifest+json";
        case "webp":
          return "image/webp";
        case "wg":
          return "application/vnd.pmi.widget";
        case "wgt":
          return "application/widget";
        case "wks":
          return "application/vnd.ms-works";
        case "wm":
          return "video/x-ms-wm";
        case "wma":
          return "audio/x-ms-wma";
        case "wmd":
          return "application/x-ms-wmd";
        case "wmf":
          return "application/x-msmetafile";
        case "wml":
          return "text/vnd.wap.wml";
        case "wmlc":
          return "application/vnd.wap.wmlc";
        case "wmls":
          return "text/vnd.wap.wmlscript";
        case "wmlsc":
          return "application/vnd.wap.wmlscriptc";
        case "wmv":
          return "video/x-ms-wmv";
        case "wmx":
          return "video/x-ms-wmx";
        case "wmz":
          return "application/x-ms-wmz";
        case "woff":
          return "application/font-woff";
        case "woff2":
          return "application/font-woff2";
        case "wpd":
          return "application/vnd.wordperfect";
        case "wpl":
          return "application/vnd.ms-wpl";
        case "wps":
          return "application/vnd.ms-works";
        case "wqd":
          return "application/vnd.wqd";
        case "wri":
          return "application/x-mswrite";
        case "wrl":
          return "model/vrml";
        case "wsdl":
          return "application/wsdl+xml";
        case "wspolicy":
          return "application/wspolicy+xml";
        case "wtb":
          return "application/vnd.webturbo";
        case "wvx":
          return "video/x-ms-wvx";
        case "x32":
          return "application/x-authorware-bin";
        case "x3d":
          return "model/x3d+xml";
        case "x3db":
          return "model/x3d+binary";
        case "x3dbz":
          return "model/x3d+binary";
        case "x3dv":
          return "model/x3d+vrml";
        case "x3dvz":
          return "model/x3d+vrml";
        case "x3dz":
          return "model/x3d+xml";
        case "xaml":
          return "application/xaml+xml";
        case "xap":
          return "application/x-silverlight-app";
        case "xar":
          return "application/vnd.xara";
        case "xbap":
          return "application/x-ms-xbap";
        case "xbd":
          return "application/vnd.fujixerox.docuworks.binder";
        case "xbm":
          return "image/x-xbitmap";
        case "xdf":
          return "application/xcap-diff+xml";
        case "xdm":
          return "application/vnd.syncml.dm+xml";
        case "xdp":
          return "application/vnd.adobe.xdp+xml";
        case "xdssc":
          return "application/dssc+xml";
        case "xdw":
          return "application/vnd.fujixerox.docuworks";
        case "xenc":
          return "application/xenc+xml";
        case "xer":
          return "application/patch-ops-error+xml";
        case "xfdf":
          return "application/vnd.adobe.xfdf";
        case "xfdl":
          return "application/vnd.xfdl";
        case "xht":
          return "application/xhtml+xml";
        case "xhtml":
          return "application/xhtml+xml";
        case "xhvml":
          return "application/xv+xml";
        case "xif":
          return "image/vnd.xiff";
        case "xla":
          return "application/vnd.ms-excel";
        case "xlam":
          return "application/vnd.ms-excel.addin.macroenabled.12";
        case "xlc":
          return "application/vnd.ms-excel";
        case "xlf":
          return "application/x-xliff+xml";
        case "xlm":
          return "application/vnd.ms-excel";
        case "xloc":
          return "text/vnd.rim.location.xloc";
        case "xls":
          return "application/vnd.ms-excel";
        case "xlsb":
          return "application/vnd.ms-excel.sheet.binary.macroenabled.12";
        case "xlsm":
          return "application/vnd.ms-excel.sheet.macroenabled.12";
        case "xlsx":
          return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        case "xlt":
          return "application/vnd.ms-excel";
        case "xltm":
          return "application/vnd.ms-excel.template.macroenabled.12";
        case "xltx":
          return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
        case "xlw":
          return "application/vnd.ms-excel";
        case "xm":
          return "audio/xm";
        case "xml":
          return "application/xml";
        case "xo":
          return "application/vnd.olpc-sugar";
        case "xop":
          return "application/xop+xml";
        case "xpi":
          return "application/x-xpinstall";
        case "xpl":
          return "application/xproc+xml";
        case "xpm":
          return "image/x-xpixmap";
        case "xpr":
          return "application/vnd.is-xpr";
        case "xps":
          return "application/vnd.ms-xpsdocument";
        case "xpw":
          return "application/vnd.intercon.formnet";
        case "xpx":
          return "application/vnd.intercon.formnet";
        case "xsl":
          return "application/xml";
        case "xslt":
          return "application/xslt+xml";
        case "xsm":
          return "application/vnd.syncml+xml";
        case "xspf":
          return "application/xspf+xml";
        case "xul":
          return "application/vnd.mozilla.xul+xml";
        case "xvm":
          return "application/xv+xml";
        case "xvml":
          return "application/xv+xml";
        case "xwd":
          return "image/x-xwindowdump";
        case "xyz":
          return "chemical/x-xyz";
        case "xz":
          return "application/x-xz";
        case "yang":
          return "application/yang";
        case "yin":
          return "application/yin+xml";
        case "z1":
          return "application/x-zmachine";
        case "z2":
          return "application/x-zmachine";
        case "z3":
          return "application/x-zmachine";
        case "z4":
          return "application/x-zmachine";
        case "z5":
          return "application/x-zmachine";
        case "z6":
          return "application/x-zmachine";
        case "z7":
          return "application/x-zmachine";
        case "z8":
          return "application/x-zmachine";
        case "zaz":
          return "application/vnd.zzazz.deck+xml";
        case "zip":
          return "application/zip";
        case "zir":
          return "application/vnd.zul";
        case "zirz":
          return "application/vnd.zul";
        case "zmm":
          return "application/vnd.handheld-entertainment+xml";
        default:
          return (string) null;
      }
    }
  }
}
