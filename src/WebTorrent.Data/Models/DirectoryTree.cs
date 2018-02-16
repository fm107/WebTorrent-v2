using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingDirectoryStructureByUsingNestedSetModel.Models
{
    public class DirectoryTreeMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FileTypeEnum FileType { get; set; }
        public int ParentId { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
    }
   

    public enum FileTypeEnum
    {
        Directory = 1,
        File = 2
    }
}
