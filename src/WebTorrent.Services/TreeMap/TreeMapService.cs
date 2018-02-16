using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SavingDirectoryStructureByUsingNestedSetModel.Models;
using WebTorrent.Data.Models;

namespace SavingDirectoryStructureByUsingNestedSetModel.Services
{
    public class TreeMapService
    {
        private readonly DbContext _context;
        private readonly DbSet<Content> _directoryTreeMapSet;

        public TreeMapService()
        {
            _context = new TreeContext();
            _directoryTreeMapSet = _context.Set<DirectoryTreeMap>();
        }

        public void InitRoot()
        {
            var query = "Select top 1 * from DirectoryTreeMap";
            var result = _directoryTreeMapSet.SqlQuery(query).FirstOrDefault();
            if (result == null)
            {
                var newNode = new DirectoryTreeMap
                {
                    Lft = 1,
                    Rgt = 2,
                    ParentId = 0
                };
                _directoryTreeMapSet.Add(newNode);
                _context.SaveChanges();
            }
        }

        public void InsertNewNode(int parentNodeId, string nodeName, FileTypeEnum fileType)
        {
            _context.Database.ExecuteSqlCommand("Exec InsertNewNode @p0, @p1, @p2", parentNodeId, nodeName, fileType);
        }

        public DirectoryTreeMap GetById(int id)
        {
            var query = $"SELECT top 1 * FROM dbo.DirectoryTreeMap WHERE Id = '{id}'";
            var result = _directoryTreeMapSet.SqlQuery(query).FirstOrDefault();
            return result;
        }

        public void MoveToRightSide(DirectoryTreeMap currentNode, DirectoryTreeMap parentNode)
        {
            _context.Database.ExecuteSqlCommand("Exec [MoveNode] @p0, @p1, @p2, @p3", currentNode.Id, parentNode.Id,
                currentNode.Lft, currentNode.Rgt);
        }

        public void MoveToLeftSide(DirectoryTreeMap currentNode, DirectoryTreeMap parentNode)
        {
            _context.Database.ExecuteSqlCommand("Exec [MoveNode] @p0, @p1, @p2, @p3", currentNode.Id, parentNode.Id,
                currentNode.Lft, currentNode.Rgt);
        }

        public void DeleteNode(DirectoryTreeMap currentNode)
        {
            //only delete if node is leaf
            if ((currentNode.Rgt - currentNode.Lft + 1) / 2 == 1)
            {
                _context.Database.ExecuteSqlCommand("Exec [DeleteNode] @p0", currentNode.Id);
            }
        }

        public void DeleteTree()
        {
            _context.Database.ExecuteSqlCommand("DELETE FROM dbo.DirectoryTreeMap");
        }

        public List<DirectoryTreeMap> GetByName(string name)
        {
            var query = $"SELECT * FROM dbo.DirectoryTreeMap WHERE Name = '{name}'";
            var result = _directoryTreeMapSet.SqlQuery(query).ToList();
            return result;
        }

        public void DisplayRootTree()
        {
            // retrieve the left and right value of the $root node  
            var query = $"SELECT TOP 1 * FROM dbo.DirectoryTreeMap ORDER BY id;";
            var result1 = _directoryTreeMapSet.SqlQuery(query).AsNoTracking().FirstOrDefault();
            if (result1 != null)
            {
                DisplayTree(result1);
            }
        }

        public void DisplayTree(DirectoryTreeMap root)
        {
            var right = new List<int>();

            // now, retrieve all descendants of the $root node  

            var query = "SELECT * FROM DirectoryTreeMap " +
                        $"WHERE lft BETWEEN {root.Lft} AND {root.Rgt} ORDER BY [lft] ASC;";
            var children = _directoryTreeMapSet.SqlQuery(query).AsNoTracking().ToList();
            // display each row  
            foreach (var child in children)
            {
                // only check stack if there is one  
                if (right.Count > 0)
                {
                    // check if we should remove a node from the stack  
                    while (right[right.Count - 1] < child.Rgt)
                    {
                        right.RemoveAt(right.Count - 1);
                    }
                }

                for (var i = 0; i < right.Count; i++)
                {
                    Console.Write("  ");
                }

                // display indented node title                  
                Console.WriteLine(child.Name + $" ({child.Lft}:{child.Rgt})");

                // add this node to the stack  
                right.Add(child.Rgt);
                //Console.WriteLine("Right: " + child.Rgt);
            }
        }
    }
}