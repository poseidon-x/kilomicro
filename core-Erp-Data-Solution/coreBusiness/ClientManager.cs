using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;

namespace coreBusiness
{
    public class ClientManager:IClientManager
    {
        public void SaveClientData(client cl)
        {
            using (var le = new coreLoansEntities())
            {
                le.Entry(cl).State = (cl.clientID > 0) ? EntityState.Modified : EntityState.Added;

                foreach (var r in cl.clientAddresses)
                {
                    le.Entry(r).State = (r.clientAddressID > 0) ? EntityState.Modified : EntityState.Added;
                    le.Entry(r.address).State = (r.address.addressID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientBankAccounts)
                {
                    le.Entry(r).State = (r.clientBankAccountID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientBusinessActivities)
                {
                    le.Entry(r).State = (r.clientBusActID > 0) ? EntityState.Modified : EntityState.Added;
                }

                if (cl.clientCompany != null)
                {
                    le.Entry(cl.clientCompany).State = (cl.clientCompany.clientID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientDocuments)
                {
                    le.Entry(r).State = (r.clientDocumentID > 0) ? EntityState.Modified : EntityState.Added;
                    le.Entry(r.document).State = (r.document.documentID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientEmails)
                {
                    le.Entry(r).State = (r.clientEmailID > 0) ? EntityState.Modified : EntityState.Added;
                    le.Entry(r.email).State = (r.email.emailID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientImages)
                {
                    le.Entry(r).State = (r.clientImageID > 0) ? EntityState.Modified : EntityState.Added;
                    le.Entry(r.image).State = (r.image.imageID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientLiabilities)
                {
                    le.Entry(r).State = (r.clientLiabilityID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.clientPhones)
                {
                    le.Entry(r).State = (r.clientPhoneID > 0) ? EntityState.Modified : EntityState.Added;
                    le.Entry(r.phone).State = (r.phone.phoneID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.employeeCategories)
                {
                    le.Entry(r).State = (r.employeeCategoryID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.groupCategories)
                {
                    le.Entry(r).State = (r.groupCategoryID > 0) ? EntityState.Modified : EntityState.Added;
                }

                if (cl.idNo != null)
                {
                    le.Entry(cl.idNo).State = (cl.idNo.idNoID > 0) ? EntityState.Modified : EntityState.Added;
                }

                if (cl.idNo != null)
                {
                    le.Entry(cl.idNo).State = (cl.idNo.idNoID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.microBusinessCategories)
                {
                    le.Entry(r).State = (r.microBusinessCategoryID > 0) ? EntityState.Modified : EntityState.Added;
                }

                foreach (var r in cl.smeCategories)
                {
                    le.Entry(r).State = (r.smeCategoryID > 0) ? EntityState.Modified : EntityState.Added;
                    if (r.address != null)
                    {
                        le.Entry(r.address).State = (r.address.addressID > 0) ? EntityState.Modified : EntityState.Added;
                    }
                    if (r.address1 != null)
                    {
                        le.Entry(r.address1).State = (r.address1.addressID > 0) ? EntityState.Modified : EntityState.Added;
                    }
                    foreach (var r2 in r.smeDirectors)
                    {
                        le.Entry(r2).State = (r2.smeDirectorID > 0) ? EntityState.Modified : EntityState.Added;
                    }
                }

                foreach (var r in cl.staffCategory1)
                {
                    le.Entry(r).State = (r.staffCategoryID > 0) ? EntityState.Modified : EntityState.Added;
                    foreach (var r2 in r.staffCategoryDirectors)
                    {
                        le.Entry(r2).State = (r2.staffCategoryDirectorID > 0) ? EntityState.Modified : EntityState.Added;
                    }
                }

                var clis = le.clientImages.Where(p => p.clientID == cl.clientID).ToList();
                foreach (var cli in clis)
                {
                    if (cl.clientImages.FirstOrDefault(p => p.clientImageID == cli.clientImageID) == null)
                    {
                        le.Entry(cli).State = EntityState.Deleted; 
                        if (cli.image != null)
                        {
                            le.Entry(cli.image).State = EntityState.Deleted;
                        }
                    }
                }

                var clds = le.clientDocuments.Where(p => p.clientID == cl.clientID).ToList();
                foreach (var cld in clds)
                {
                    if (cl.clientDocuments.FirstOrDefault(p => p.clientDocumentID == cld.clientDocumentID) == null)
                    {
                        le.Entry(cld).State = EntityState.Deleted;
                        if (cld.document != null)
                        {
                            le.Entry(cld.document).State = EntityState.Deleted;
                        }
                    }
                }

                var clns = le.nextOfKins.Where(p => p.clientID == cl.clientID).ToList();
                foreach (var cln in clns)
                {
                    if (cl.nextOfKins.FirstOrDefault(p => p.nextOfKinID == cln.nextOfKinID) == null)
                    {
                        le.Entry(cln).State = EntityState.Deleted;
                        if (cln.idNo != null)
                        {
                            le.Entry(cln.idNo).State = EntityState.Deleted;
                        }
                        if (cln.image != null)
                        {
                            le.Entry(cln.image).State = EntityState.Deleted;
                        }
                        if (cln.phone != null)
                        {
                            le.Entry(cln.phone).State = EntityState.Deleted;
                        }
                        if (cln.email != null)
                        {
                            le.Entry(cln.email).State = EntityState.Deleted;
                        }
                    }
                }

                var clbs = le.clientBankAccounts.Where(p => p.clientID == cl.clientID).ToList();
                foreach (var clb in clbs)
                {
                    if (cl.clientBankAccounts.FirstOrDefault(p => p.clientBankAccountID == clb.clientBankAccountID)==null)
                    {
                        le.Entry(clb).State = EntityState.Deleted;
                    }
                }

                le.SaveChanges();
            }
        }

        public client FetchClientData(int clientID)
        {
            using (var le = new coreLoansEntities())
            {
                var cl = le.clients 
                    .Include(p=> p.branch)  
                    .Include(p=> p.category)
                    .Include(p=> p.clientAddresses)
                    .Include(p=> p.clientAddresses.Select(q=> q.address))
                    .Include(p=> p.clientBankAccounts)
                    .Include(p=> p.clientCompany)
                    .Include(p=> p.clientCompany.address)
                    .Include(p=> p.clientCompany.email)
                    .Include(p=>p.clientCompany.phone)
                    .Include(p=> p.clientDocuments)
                    .Include(p=> p.clientDocuments.Select(q=> q.document))
                    .Include(p=>p.clientEmails)
                    .Include(p=> p.clientEmails.Select(q=>q.email))
                    .Include(p=> p.clientImages)
                    .Include(p=> p.clientImages.Select(q=> q.image))
                    .Include(p=> p.clientLiabilities)
                    .Include(p=> p.clientPhones)
                    .Include(p=> p.clientPhones.Select(q=> q.phone))
                    .Include(p=> p.employeeCategories)
                    .Include(p => p.employeeCategories.Select(q => q.employer))
                    .Include(p => p.groupCategories)
                    .Include(p=> p.groupCategories.Select(q=> q.group))
                    .Include(p=> p.groupCategories.Select(q=> q.group.address))
                    .Include(p=> p.idNo)
                    .Include(p=> p.idNo1)
                    .Include(p=> p.industry)
                    .Include(p=> p.maritalStatu)
                    .Include(p=> p.microBusinessCategories)
                    .Include(p=> p.microBusinessCategories.Select(q=> q.lineOfBusiness1))
                    .Include(p=> p.nextOfKins)
                    .Include(p=> p.nextOfKins.Select(q=> q.email))
                    .Include(p=> p.nextOfKins.Select(q=> q.idNo))
                    .Include(p=> p.nextOfKins.Select(q=> q.image))
                    .Include(p=> p.nextOfKins.Select(q=> q.phone))
                    .Include(p=> p.sector)
                    .Include(p=> p.smeCategories)
                    .Include(p=> p.smeCategories.Select(q=> q.address))
                    .Include(p=> p.smeCategories.Select(q=>q.address1))
                    .Include(p=> p.smeCategories.Select(q=> q.smeDirectors))
                    .Include(p => p.staffCategory1)
                    .Include(p => p.staffCategory1.Select(q => q.employer))
                    .Include(p => p.staffCategory1.Select(q => q.employerDepartment))
                    .Include(p => p.staffCategory1.Select(q => q.staffCategoryDirectors)) 
                    .AsNoTracking()
                    .First(p => p.clientID == clientID);
                return cl;
            }
        }
    }
}
