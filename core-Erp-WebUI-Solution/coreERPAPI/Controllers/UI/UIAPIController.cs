using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using coreERP.Models;

namespace coreERP.Controllers.UI
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class UIAPIController : ApiController
    {
        private coreLogic.coreSecurityEntities sent = new coreLogic.coreSecurityEntities();

        [HttpGet]
        public List<MenuData> GetMenus(string authToken)
        {
            List<MenuData> menus = new List<MenuData>();
            var items = sent.modules.Where(p=> p.visible==true).ToList();

            GetMenus(menus, items);
            RenderMenu(menus, authToken);

            return menus;
        }

        [HttpGet]
        public List<SideMenuItem> GetMenuHeaders(string authToken, int menuType)
        {
            List<SideMenuItem> menus = new List<SideMenuItem>();

            if (menuType == 1)
            {
                GetSavLoansMenuHeaders(menus, menuType);
            }
            else if (menuType == 2)
            {
                GetGLMenuHeaders(menus, menuType);
            }

            return menus;
        }

        public List<SideMenuItem> GetMenuDetails(string authToken, int menuType, int itemId)
        {
            List<SideMenuItem> menus = new List<SideMenuItem>();

            if (menuType == 1)
            {
                if (itemId == 1)
                {
                    var texts = new string[] { "Cashier Home", "Cashier Detailed Report", "Cashier Summary Report", 
                    "Print Client Loan Statement", "Print Term Deposit Statement", "Print Savings Account Statement"};
                    var links = new string[] { "/ln/cashier/default3.aspx", "/ln/reports/cashier.aspx",
                    "/ln/reports/cashier2.aspx", "/ln/reports/statement.aspx", "/ln/depositReports/statement.aspx",
                    "/ln/savingReports/statement.aspx"};
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 2)
                {
                    var texts = new string[] { "Create a Cashier Account", "Open or Close a Cashier", 
                        "Post Cashier Transactions", 
                        "Clear Checks Posted By Cashier"};
                    var links = new string[] { "/ln/setup/cashiersTill.aspx", "/ln/setup/openTill.aspx",
                        "/ln/setup/postTill.aspx", "/ln/setup/closeCheckTill.aspx"};
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 3)
                {
                    var texts = new string[] { "Enter a new Client", "Edit an existing Client" };
                    var links = new string[] { "/ln/client/client.aspx", "/ln/client/default.aspx" };
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 4)
                {
                    var texts = new string[] { "Enter a new Loan", "Edit an existing Loan" };
                    var links = new string[] { "/ln/loans/loan.aspx", "/ln/loans/default.aspx" };
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 5)
                {
                    var texts = new string[] { "Enter a new Savings Account", "Edit an existing Savings Account" };
                    var links = new string[] { "/ln/saving/saving.aspx", "/ln/saving/default.aspx" };
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 6)
                {
                    var texts = new string[] { "Enter a new Term Deposit", "Edit an existing erm Deposit" };
                    var links = new string[] { "/ln/deposit/deposit.aspx", "/ln/deposit/default.aspx" };
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId, 
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass="sideMenuItem"
                        });
                    }
                }
            }
            else if (menuType == 2)
            {
                if (itemId == 1)
                {
                    var texts = new string[] { "Enter Local Currency Transactions", "View Journal Transactions",  
                        "Post Temporary Journal"};
                    var links = new string[] { "/gl/journal/default.aspx", "/gl/journal/view.aspx",
                        "/gl/journal/post.aspx"};
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId,
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 2)
                {
                    var texts = new string[] { "Enter Petty Cash", "Post Petty Cash" };
                    var links = new string[] { "/gl/pc/pc.aspx", "/gl/pc/psot.aspx" };
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId,
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }
                else if (itemId == 3)
                {
                    var texts = new string[] { "Balance Sheet", "Trial Balance",
                        "Income Statement", "Cross Tab Income Statement", "Journal Transaction Details By Account",
                        "Comparative Income Statement"};
                    var links = new string[] { "/gl/reports/bal_sht_std.aspx", "/gl/reports/trial_bal_std.aspx",
                        "/gl/reports/op_stmt_std.aspx", "/gl/reports/op_stmt_ct.aspx",
                        "/gl/reports/tx_by_acc.aspx", "/gl/reports/op_stmt_2yrs.aspx"};
                    for (int i = 0; i < texts.Length; i++)
                    {
                        menus.Add(new SideMenuItem
                        {
                            itemId = itemId,
                            imageUrl = "/images/new.jpg",
                            text = texts[i],
                            menuType = menuType,
                            url = links[i],
                            cssClass = "sideMenuItem"
                        });
                    }
                }    
            }

            return menus;
        }

        public void GetSavLoansMenuHeaders(List<SideMenuItem> menus, int menuType)
        {
            var imgUrls = new string[]{
                "/images/cashier.jpg",
                "/images/cashier2.jpg",
                "/images/client.jpg",
                "/images/loan.jpg",
                "/images/saving.jpg",
                "/images/deposit.jpg"
            };
            var texts = new string[]{
                "Cashier",
                "Cashier Administration",
                "Clients",
                "Loans",
                "Savings",
                "Deposits"
            };
            for (int i = 0; i < texts.Length; i++)
            {
                var item = new SideMenuItem 
                {
                    itemId = i + 1,
                    expanded = (i==0),
                    imageUrl = imgUrls[i],
                    text = texts[i],
                    menuType = menuType,
                    items = GetMenuDetails("", menuType, i + 1),
                    cssClass="sideMenuItem"
                };
                menus.Add(item);
            }
        }

        public void GetGLMenuHeaders(List<SideMenuItem> menus, int menuType)
        {
            var imgUrls = new string[]{
                "/images/cashier.jpg",
                "/images/cashier2.jpg",
                "/images/chart_of_accounts/account.jpg" 
            };
            var texts = new string[]{
                "Journal/Bookkeeping",
                "Petty Cash",
                "Financial Reports" 
            };
            for (int i = 0; i < texts.Length; i++)
            {
                var item = new SideMenuItem
                {
                    itemId = i + 1,
                    expanded = (i == 0),
                    imageUrl = imgUrls[i],
                    text = texts[i],
                    menuType = menuType,
                    items = GetMenuDetails("", menuType, i + 1),
                    cssClass = "sideMenuItem"
                };
                menus.Add(item);
            }
        }

        private void GetMenus(List<MenuData> menus, List<coreLogic.modules> source)
        {
            var items = source.Where(p => p.parent_module_id == null).OrderBy(p=> p.sort_value).ToList();
            foreach (var item in items)
            {
                var childItems = source.Where(p => p.parent_module_id == item.module_id).OrderBy(p => p.sort_value).ToList();
                var menu = new MenuData
                {
                    text = item.module_name,
                    url = (childItems.Count == 0) ? item.url.Replace("~","") : ""
                };
                if (childItems.Count > 0)
                {
                    menu.items = new List<MenuData>();
                }
                menus.Add(menu);
                GetMenus(childItems, item, menu, source);
            }
        }

        private void GetMenus(List<coreLogic.modules> items, coreLogic.modules item, MenuData menu, List<coreLogic.modules> source)
        {
            foreach (var it in items)
            {
                var childItems = source.Where(p => p.parent_module_id == it.module_id).OrderBy(p => p.sort_value).ToList();
                var menuChild = new MenuData
                {
                    text = it.module_name,
                    url = (childItems.Count == 0) ? it.url.Replace("~", "") : ""
                };
                if (childItems.Count > 0)
                {
                    menuChild.items = new List<MenuData>();
                }
                menu.items.Add(menuChild);
                GetMenus(childItems, it, menuChild, source);
            }
        }
        
        private void RenderMenu(List<MenuData> menus, string authToken)
        {
            try
            {
                var token = sent.authTokens.FirstOrDefault(p => p.token == authToken);
                if (token != null)
                {
                    MenuData item = new MenuData
                    {
                        text = "Hello",
                        url = "",
                        cssClass="pull-right",
                        items=new List<MenuData>()
                    };
                    menus.Add(item);
                    MenuData item2 = new MenuData { text = "Logout", url = "/security/logout.aspx" };
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Change Password", url = "/profile/settings.aspx" };
                    item.items.Add(item2);

                    item2 = new MenuData
                    {
                        text = "Interface Preference",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item.items.Add(item2);

                    var item4 = new MenuData
                    {
                        text = "Modern &amp; Metro",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item2.items.Add(item4);
                    var item3 = new MenuData { text = "Modern", url = "/dash/default.aspx?skin=Bootstrap" };
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Metro", url="/dash/default.aspx?skin=Metro"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Metro Touch", url="/dash/default.aspx?skin=MetroTouch"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Black", url="/dash/default.aspx?skin=Black"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Metro Black Touch", url="/dash/default.aspx?skin=BlackMetroTouch"}; 
                    item4.items.Add(item3);

                    item4 = new MenuData
                    {
                        text = "Office Look",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item2.items.Add(item4); 
                    item3 = new MenuData { text = "Outlook", url="/dash/default.aspx?skin=Outlook"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Office Blue", url="/dash/default.aspx?skin=Office2010Blue"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Office Silver", url="/dash/default.aspx?skin=Office2010Silver"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Office Black", url="/dash/default.aspx?skin=Office2010Black"}; 
                    item4.items.Add(item3);

                    item4 = new MenuData
                    {
                        text = "Windows Look",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item2.items.Add(item4); 
                    item3 = new MenuData { text = "Vista", url="/dash/default.aspx?skin=Vista"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Windows 7", url="/dash/default.aspx?skin=Windows7"}; 
                    item4.items.Add(item3);

                    item4 = new MenuData
                    {
                        text = "The New Web",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item2.items.Add(item4); 
                    item3 = new MenuData { text = "Telerik", url="/dash/default.aspx?skin=Telerik"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Silky Smooth", url="/dash/default.aspx?skin=Silk"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Web Blue", url="/dash/default.aspx?skin=WebBlue"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Web 2.0", url="/dash/default.aspx?skin=Web20"}; 
                    item4.items.Add(item3);

                    item4 = new MenuData
                    {
                        text = "Simplified",
                        url = "",
                        items = new List<MenuData>()
                    };
                    item2.items.Add(item4); 
                    item3 = new MenuData { text = "Default", url="/dash/default.aspx?skin=Default"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Glow", url="/dash/default.aspx?skin=Glow"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Simple", url="/dash/default.aspx?skin=Simple"}; 
                    item4.items.Add(item3);
                    item3 = new MenuData { text = "Sunset", url="/dash/default.aspx?skin=Sunset"}; 
                    item4.items.Add(item3);

                    try
                    {
                        var admRole = token.user.user_roles.FirstOrDefault(p => p.roles.role_name.ToLower() == "itadmin");
                        if (admRole != null)
                        {
                            item2 = new MenuData { text = "Users Administration", url = "/admin/users.aspx" };
                            item.items.Add(item2);
                            item2 = new MenuData { text = "Roles Administration", url = "/admin/" };
                            item.items.Add(item2);
                            item2 = new MenuData { text = "Authorizations and Modules", url = "/admin/modules.aspx" };
                            item.items.Add(item2);
                        }

                        if (token.user.user_name.ToLower() == "coreadmin")
                        {
                            item2 = new MenuData { text = "Super Authorizations and Modules", url = "/admin/modulesSuper.aspx" };
                            item.items.Add(item2);
                        }
                    }
                    catch (Exception y) { }

                    item = new MenuData
                    {
                        text = "Links",
                        url = "",
                        cssClass = "pull-right",
                        items = new List<MenuData>()
                    };
                    menus.Add(item);
                    item2 = new MenuData { text = "Home", url = "/dash/default.aspx" };                     
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Loan Dashboard", url = "/dash/jdefault2.aspx" };
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Investment Dashboard", url = "/dash/jdefault3.aspx" };
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Loan Performance Analysis", url = "/dash/jdefault.aspx" };
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Alerts", url = "/dash/sldash.aspx" };
                    item.items.Add(item2);
                    item2 = new MenuData { text = "Help", url = "/help/Build html documentation/index.html" };
                    item.items.Add(item2);

                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
