using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIS_QSF.Data;
using SIS_QSF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;

namespace SIS_QSF.Controllers
{ 
    [Authorize]
    public class SolicitudController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        [Obsolete]
        private readonly IHostingEnvironment _IHostingEnvironment;
        [Obsolete]
        public SolicitudController(ApplicationDbContext context, IFileProvider fileProvider, IHostingEnvironment env)
        {
            _context = context;
            _fileProvider = fileProvider;
            _IHostingEnvironment = env;
        }

        // GET: Solicitud
        [Authorize(Roles = "admin, cali")]
        public async Task<IActionResult> Index(string searchString)
        {
            var solicitudes = from m in _context.Solicituds
                              select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                solicitudes = solicitudes.Where(s => s.Nombre.Contains(searchString));
                
            }

            return View(await solicitudes.Include(i => i.Depa).ToListAsync());
        }
        
        [Authorize(Roles = "admin, cali")]
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Solicitud/Details/5
        [Authorize(Roles = "cali")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // GET: Solicitud/Create
        [Authorize(Roles = "user")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Solicitud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        [Obsolete]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Telefono,Email,Descripcion")] Solicitud solicitud, IFormFile file, int depa)
        {
            if (ModelState.IsValid)
            {
                try{

                
                
                if (file != null || file.Length != 0)
                {
                    //Codigo para subir imagenes.
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    var newFileName = solicitud.Id + "_" + String.Format("{0:d}", (DateTime.Now.Ticks / 10) % 100000000) + fileInfo.Extension;
                    var webPath = _IHostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\imagesFilesUpload\" + newFileName);
                    var pathToSave = @"/imagesFilesUpload/" + newFileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    solicitud.ImagenPath = pathToSave;
                
                }
                
                }catch(Exception e){
                    Console.Write(e.Message);

                }
                Departamento departa = new Departamento();
                    if(depa==1){
                        departa.Id=1;
                        departa.Nombre="Calidad";
                    }else if(depa==2){
                        departa.Id=2;
                        departa.Nombre="Idiomas";
                    }else if(depa==3){
                        departa.Id=3;
                        departa.Nombre="Finanzas";
                    }else {
                        departa.Id=4;
                        departa.Nombre="Becas";
                    }
                    solicitud.Depa=departa;
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(solicitud);
        }

        // GET: Solicitud/Edit/5
        
       [Authorize(Roles = "cali")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return View(solicitud);
        }

        // POST: Solicitud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "cali")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono,Email,Descripcion")] Solicitud solicitud)
        {
            if (id != solicitud.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudExists(solicitud.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(solicitud);
        }

        // GET: Solicitud/Delete/5
        [Authorize(Roles = "cali")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicituds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: Solicitud/Delete/5      
        [Authorize(Roles = "cali")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.Solicituds.FindAsync(id);
            _context.Solicituds.Remove(solicitud);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicituds.Any(e => e.Id == id);
        }
    }
}
