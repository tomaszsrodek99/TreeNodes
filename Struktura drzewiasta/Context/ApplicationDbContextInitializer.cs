using Struktura_drzewiasta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_drzewiasta.Context
{
    public static class ApplicationDbContextInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Database.CanConnect())
            {
                context.Database.EnsureCreated();
            }

            if (!context.TreeNodes.Any())
            {
                // Tworzenie węzłów drzewa
                var nodes = new List<TreeNode>
                {
                    new TreeNode { Name = "Root", ParentId = null },
                    new TreeNode { Name = "Dokumenty", ParentId = 1 },
                    new TreeNode { Name = "Wideo", ParentId = 1 },
                    new TreeNode { Name = "Obrazki", ParentId = 1 },
                    new TreeNode { Name = "Moje zdjęcia", ParentId = 4 }
                };

                foreach (var node in nodes)
                {
                    context.TreeNodes.Add(node);
                }

                context.SaveChanges();
            }
        }
    }
}

