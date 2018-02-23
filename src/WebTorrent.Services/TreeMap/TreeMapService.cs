using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTorrent.Data;
using WebTorrent.Data.Models;

namespace WebTorrent.Services.TreeMap
{
    public class TreeMapService
    {
        private readonly DbContext _context;
        private readonly DbSet<Content> _contentTreeMapSet;

        public TreeMapService(ApplicationDbContext context)
        {
            _context = context;
            _contentTreeMapSet = _context.Set<Content>();
        }

        public void InitRoot()
        {
            
            var query = "Select top 1 * from dbo.Content";
            var result = _contentTreeMapSet.FromSql(query).FirstOrDefault();
            if (result == null)
            {
                var newNode = new Content()
                {
                    Lft = 1,
                    Rgt = 2,
                    ParentId = 0
                };
                _contentTreeMapSet.Add(newNode);
                _context.SaveChanges();
            }
        }

        public async Task InsertNewNode(Content content,int parentNodeId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                try
                {
                    ((ApplicationDbContext)_context).Content.Add(content);
                    await _context.SaveChangesAsync();

                    _context.Database.ExecuteSqlCommand("Exec InsertNewNode @p0", parentNodeId);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    transaction.Rollback();
                }
            }
        }

        public void AddChild(Content rootNode, Content child)
        {
            MoveToLeftSide(rootNode, child);
        }

        public Content GetById(int id)
        {
            var query = $"SELECT top 1 * FROM dbo.Content WHERE Id = '{id}'";
            var result = _contentTreeMapSet.FromSql(query).FirstOrDefault();
            return result;
        }

        public void MoveToRightSide(Content currentNode, Content parentNode)
        {
            _context.Database.ExecuteSqlCommand("Exec [MoveNode] @p0, @p1, @p2, @p3", currentNode.Id, parentNode.Id,
                currentNode.Lft, currentNode.Rgt);
        }

        public void MoveToLeftSide(Content currentNode, Content parentNode)
        {
            _context.Database.ExecuteSqlCommand("Exec [MoveNode] @p0, @p1, @p2, @p3", currentNode.Id, parentNode.Id,
                currentNode.Lft, currentNode.Rgt);
        }

        public void DeleteNode(Content currentNode)
        {
            //only delete if node is leaf
            if ((currentNode.Rgt - currentNode.Lft + 1) / 2 == 1)
            {
                _context.Database.ExecuteSqlCommand("Exec [DeleteNode] @p0", currentNode.Id);
            }
        }

        public void DeleteTree()
        {
            _context.Database.ExecuteSqlCommand("DELETE FROM dbo.Content");
        }

        public List<Content> GetByHash(string hash)
        {
            var query = $"SELECT * FROM dbo.Content WHERE Hash = '{hash}'";
            var result = _contentTreeMapSet.FromSql(query).ToList();
            return result;
        }

        public void DisplayRootTree()
        {
            // retrieve the left and right value of the $root node  
            var query = $"SELECT TOP 1 * FROM dbo.Content ORDER BY Id";
            var result1 = _contentTreeMapSet.FromSql(query).AsNoTracking().FirstOrDefault();
            if (result1 != null)
            {
                DisplayTree(result1);
            }
        }

        public void DisplayTree(Content root)
        {
            var right = new List<int>();

            // now, retrieve all descendants of the $root node  

            var query = "SELECT * FROM dbo.Content " +
                        $"WHERE lft BETWEEN {root.Lft} AND {root.Rgt} ORDER BY [lft] ASC;";
            var children = _contentTreeMapSet.FromSql(query).AsNoTracking().ToList();
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
                Console.WriteLine(child.TorrentName + $" ({child.Lft}:{child.Rgt})");

                // add this node to the stack  
                right.Add(child.Rgt);
                //Console.WriteLine("Right: " + child.Rgt);
            }
        }
    }
}