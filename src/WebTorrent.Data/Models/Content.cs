using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebTorrent.Model
{
    public class Content : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string TorrentName { get; set; }
        public string Hash { get; set; }
        public bool IsInProgress { get; set; }
        public string CurrentFolder { get; set; }
        public string ParentFolder { get; set; }
        public virtual List<FileSystemItem> FsItems { get; set; }
    }

    public class FileSystemItem : IEntity
    {
        public int Id { get; set; }

        public bool IsStreaming { get; set; }
        public string Stream { get; set; }
        public string DownloadPath { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime LastChanged { get; set; }
        public string Type { get; set; }
    }
}