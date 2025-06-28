﻿using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    // Implementeert IPartRepository interface
    public class PartRepository : IPartRepository
    {
        private readonly MatrixIncDbContext _context;

        public PartRepository(MatrixIncDbContext context) 
        {
            _context = context; 
        }   

        // Voegt een nieuw onderdeel toe aan de database
        public void AddPart(Part part)
        {
            _context.Parts.Add(part);
            _context.SaveChanges();
        }

        // Verwijdert een onderdeel uit de database
        public void DeletePart(Part part)
        {
            _context.Parts.Remove(part);
            _context.SaveChanges();
        }

        // Haalt alle onderdelen op
        public IEnumerable<Part> GetAllParts()
        {
            return _context.Parts;            
        }

        // Zoekt een onderdeel op basis van ID
        public Part? GetPartById(int id)
        {
            return _context.Parts.FirstOrDefault(p => p.Id == id);
        }

        // Werkt een bestaand onderdeel bij in de database
        public void UpdatePart(Part part)
        {
            _context.Parts.Update(part);
            _context.SaveChanges();
        }
    }
}
