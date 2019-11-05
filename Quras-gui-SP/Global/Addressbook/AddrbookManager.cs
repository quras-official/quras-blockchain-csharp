using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras_gui_SP.Global.Addressbook
{
    class AddrbookManager
    {
        private readonly string path;

        public string DbPath => path;

        public readonly Dictionary<string, Addrbook> AddrBooks;

        public AddrbookManager(string path, bool create)
        {
            this.path = path;
            
            if (create)
            {
                BuildDatabase();
            }
            else
            {
                this.AddrBooks = LoadAddressBooks().ToDictionary(p => p.Address);
            }
        }

        public IEnumerable<Addrbook> LoadAddressBooks()
        {
            using (AddrbookDbContext ctx = new AddrbookDbContext(DbPath))
            {
                foreach (Addrbook item in ctx.Addrbooks.Select(p => p))
                {
                    yield return item;
                }
            }
        }

        public bool DeleteAddress(string address)
        {
            using (AddrbookDbContext ctx = new AddrbookDbContext(DbPath))
            {
                Addrbook addrbook = ctx.Addrbooks.FirstOrDefault(p => p.Address.SequenceEqual(address));
                if (address != null)
                {
                    ctx.Addrbooks.Remove(addrbook);
                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                    }
                }
            }
            return true;
        }

        public void SaveStoredData(string contact_name, string address)
        {
            using (AddrbookDbContext ctx = new AddrbookDbContext(DbPath))
            {
                SaveStoredData(ctx, contact_name, address);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }

        private static void SaveStoredData(AddrbookDbContext ctx, string contact_name, string address)
        {
            Addrbook addrBook = ctx.Addrbooks.FirstOrDefault(p => p.Address == address);
            if (addrBook == null)
            {
                ctx.Addrbooks.Add(new Addrbook
                {
                    ContactName = contact_name,
                    Address = address
                });
            }
            else
            {
                addrBook.ContactName = contact_name;
            }
        }

        public void BuildDatabase()
        {
            using (AddrbookDbContext ctx = new AddrbookDbContext(DbPath))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }
    }
}
