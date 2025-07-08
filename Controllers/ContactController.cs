using System.Net.Mail; // For SMTP
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Data;
using MyPortfolio.Models;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: show the contact info + form
    public IActionResult Index()
    {
        int contactId = 4; // your contact info record id
        var contact = _context.ContactInfos.FirstOrDefault(c => c.Id == contactId);
        ViewBag.ContactInfo = contact;
        if (contact == null) return NotFound();
        
        return View(contact);
    }

    // POST: receive contact form submission
    [HttpPost]
    public async Task<IActionResult> Index(ContactFormModel model, [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await emailService.SendEmailAsync(model.Name, model.Email, model.Message);
            TempData["MessageSent"] = "Your message has been sent successfully!";
        }
        catch (Exception ex)
        {
            TempData["MessageSent"] = $"Failed to send message: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}
