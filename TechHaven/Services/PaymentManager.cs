using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using TechHaven.Data;
using TechHaven.Models;

namespace TechHaven.Services
{
	public class PaymentManager
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<Customer> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public PaymentManager(ApplicationDbContext db, UserManager<Customer> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IEnumerable<PaymentMethod>> GetUserCards()
		{
			var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
			var userCards = await _db.PaymentMethod.Where(p => p.CustomerId == usrId).ToListAsync();

			if (userCards.Any())
			{
				return userCards;
			}
			else return new List<PaymentMethod>();
		}
		
		public async Task<bool> IsOwnedByUser(string creditCardNumber)
		{
			var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
			var userCards = await _db.PaymentMethod.Where(p => p.CustomerId == usrId).ToListAsync();

			if (userCards.Any())
			{
				foreach(var card in userCards)
				{
					if (card.creditCardNumber.Equals(creditCardNumber))
					{
						return true;
					}
				}
			}
			return false;
		}

		public async Task AddNewCard(string cardNumber) {
			if (await IsOwnedByUser(cardNumber))
			{
				return;
			}

			var usrId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
			var usr = await _db.Customer
				.Include(c => c.PaymentMethods)
				.FirstAsync(u => u.Id == usrId);

			if (usr== null) { throw new NullReferenceException("User is null!"); }
			if (usr.PaymentMethods == null)
			{
				usr.PaymentMethods = new List<PaymentMethod>();
			}
			usr.PaymentMethods.Add(new PaymentMethod() { creditCardNumber = cardNumber });
			await _db.SaveChangesAsync();
		}
	}
}
