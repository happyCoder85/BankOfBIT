﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankOfBit_JS.Data;
using BankOfBit_JS.Models;

namespace BankOfBit_JS.Controllers
{
    public class NextTransactionsController : Controller
    {
        private BankOfBit_JSContext db = new BankOfBit_JSContext();

        // GET: NextTransactions
        public ActionResult Index()
        {
            return View(NextTransaction.GetInstance());
        }

        // GET: NextTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransaction nextTransaction = db.NextTransactions.Find(id);
            if (nextTransaction == null)
            {
                return HttpNotFound();
            }
            return View(nextTransaction);
        }

        // GET: NextTransactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NextTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextTransaction nextTransaction)
        {
            if (ModelState.IsValid)
            {
                db.NextTransactions.Add(nextTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nextTransaction);
        }

        // GET: NextTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransaction nextTransaction = db.NextTransactions.Find(id);
            if (nextTransaction == null)
            {
                return HttpNotFound();
            }
            return View(nextTransaction);
        }

        // POST: NextTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextTransaction nextTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nextTransaction);
        }

        // GET: NextTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextTransaction nextTransaction = db.NextTransactions.Find(id);
            if (nextTransaction == null)
            {
                return HttpNotFound();
            }
            return View(nextTransaction);
        }

        // POST: NextTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextTransaction nextTransaction = db.NextTransactions.Find(id);
            db.NextTransactions.Remove(nextTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
