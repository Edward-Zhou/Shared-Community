using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SharedCommunity.Data;
using SharedCommunity.Models;
using System.Linq.Expressions;
using ForumData.Pipelines.Models;

namespace SharedCommunity.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            Expression<Func<MsdnQuestionIndexEntity, bool>> firstForField1 = (x => x.Id == "02ff8391-52b0-4bf3-929c-7f7271f5cfa4");
            Expression<Func<MsdnQuestionIndexEntity, bool>> secondForField1 = (x => x.Id == "03e27109-d597-4051-9a7f-4ddaff40ebe7");
            Expression<Func<MsdnQuestionIndexEntity, bool>> thirdForField1 = (x => x.Id == "04e7792b-aa99-45f7-b84c-71dfa1d1b413");
            Expression<Func<MsdnQuestionIndexEntity, bool>> firstForField2 = (x => x.Title == "Csv import not working");
            //Expression body = Expression.OrElse(firstForField1.Body, secondForField1.Body);

            //body = Expression.OrElse(body, thirdForField1.Body);
            //body = Expression.OrElse(body, firstForField2.Body);
            //param expression
            ParameterExpression parameter = Expression.Parameter(typeof(MsdnQuestionIndexEntity), "x");
            ParameterReplacer expressionReplacer = new ParameterReplacer(parameter);
            List<string> list = new List<string>();
            list.Add("02ff8391-52b0-4bf3-929c-7f7271f5cfa4");
            list.Add("03e27109-d597-4051-9a7f-4ddaff40ebe7");
            list.Add("04e7792b-aa99-45f7-b84c-71dfa1d1b413");
            Expression body = ExressionLoop(list);
            body = expressionReplacer.Visit(body);
            
            var where = Expression.Lambda<Func<MsdnQuestionIndexEntity, bool>>(body, parameter);
            var result = _context.MsdnQuestionIndexEntity.Where(where).ToList();
            return View(await _context.ApplicationUser.ToListAsync());
        }
        private Expression ExressionLoop(List<string> parameters)
        {
            List<Expression<Func<MsdnQuestionIndexEntity, bool>>> expressions= new List<Expression<Func<MsdnQuestionIndexEntity, bool>>>();
            foreach(string parameter in parameters)
            {
                Expression<Func<MsdnQuestionIndexEntity, bool>> expression = (x => x.Id == parameter);
                expressions.Add(expression);
            }
            if (expressions.Count == 1)
            {
                return expressions[0].Body;
            }
            else if (expressions.Count == 2)
            {
                return Expression.OrElse(expressions[0].Body, expressions[1].Body);
            }
            else
            {
                Expression body = Expression.OrElse(expressions[0].Body, expressions[1].Body);
                for (int i = 2; i < expressions.Count; i++)
                {
                    body = Expression.OrElse(body, expressions[i].Body);
                }
                return body;
            }
        }
        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Avatar,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Avatar,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;

        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        protected override Expression VisitParameter
            (ParameterExpression node)
        {
            return parameter;
        }
    }

}
