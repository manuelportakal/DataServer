
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Infrastructure.Repositories
{
    public class EntryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public EntryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public List<Entry> All()
        {
            var entries = _applicationDbContext.Entries
                                                .ToList();

            return entries;
        }

        public Entry GetByDataCode(string dataCode)
        {
            var entry = _applicationDbContext.Entries
                  .FirstOrDefault(x => x.Code == dataCode);

            return entry;
        }

        public Entry Create(Entry entry)
        {
            var controlAgent = _applicationDbContext.Agents
                                    .Include(x => x.Entries)
                                    .FirstOrDefault(x => x.Id == entry.AgentId);

            if (controlAgent == null)
                return null;

            var oldEntry = controlAgent.Entries.FirstOrDefault(x => x.Code == entry.Code);

            if (oldEntry != null)
            {
                oldEntry.Value = entry.Value;
                _applicationDbContext.Entries.Update(oldEntry);
                entry = oldEntry;
            }
            else
            {
                _applicationDbContext.Entries.Add(entry);
            }

            _applicationDbContext.SaveChanges();

            return entry;
        }

        public Entry Remove(Guid id)
        {
            var entry = _applicationDbContext.Entries
                                    .FirstOrDefault(x => x.Id == id);

            if (entry != null)
            {
                _applicationDbContext.Set<Entry>().Remove(entry);
                _applicationDbContext.SaveChanges();

                return entry;
            }

            return null;
        }
    }
}
