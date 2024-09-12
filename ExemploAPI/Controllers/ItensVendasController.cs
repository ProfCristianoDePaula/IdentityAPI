using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExemploAPI.Data;
using ExemploAPI.Models;

namespace ExemploAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensVendasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItensVendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ItensVendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItensVenda>>> GetItensVenda()
        {
            return await _context.ItensVenda.ToListAsync();
        }

        // GET: api/ItensVendas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItensVenda>> GetItensVenda(Guid id)
        {
            var itensVenda = await _context.ItensVenda.FindAsync(id);

            if (itensVenda == null)
            {
                return NotFound();
            }

            return itensVenda;
        }

        // PUT: api/ItensVendas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItensVenda(Guid id, ItensVenda itensVenda)
        {
            if (id != itensVenda.ItensVendaId)
            {
                return BadRequest();
            }

            _context.Entry(itensVenda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItensVendaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItensVendas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItensVenda>> PostItensVenda(ItensVenda itensVenda)
        {
            _context.ItensVenda.Add(itensVenda);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItensVenda", new { id = itensVenda.ItensVendaId }, itensVenda);
        }

        // DELETE: api/ItensVendas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItensVenda(Guid id)
        {
            var itensVenda = await _context.ItensVenda.FindAsync(id);
            if (itensVenda == null)
            {
                return NotFound();
            }

            _context.ItensVenda.Remove(itensVenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItensVendaExists(Guid id)
        {
            return _context.ItensVenda.Any(e => e.ItensVendaId == id);
        }
    }
}
