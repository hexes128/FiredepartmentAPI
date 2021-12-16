using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FiredepartmentAPI.Dbcontext;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FiredepartmentAPI.PostInputModles;
using Microsoft.EntityFrameworkCore;
using static FiredepartmentAPI.Dbcontext.FiredepartmentDbContext;
using FiredepartmentAPI.DbModels;
using Microsoft.AspNetCore.Authorization;
using NETCore.MailKit.Core;
using ZXing;
using ZXing.Common;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using Spire.Doc;
using System.IO;
using System.Drawing.Imaging;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using MimeKit;
using NETCore.MailKit;

namespace FiredepartmentAPI.Controllers
{
    [Route("{controller}/{action}")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly FiredepartmentDbContext Context;
        private readonly IMailKitProvider provider;

        public ItemController(FiredepartmentDbContext context, IMailKitProvider provider)
        {
            Context = context;
            this.provider = provider;
        }

        [HttpGet]
        [Authorize("API")]
        public IActionResult GetItem()
        {


            var items = Context.PlaceTable.Include(x => x.PriorityList).
                ThenInclude(x => x.FireitemList).ToArray();


            return Ok(items);
        }
        [HttpPost]
        [Authorize("API")]
        public async Task<IActionResult> ChangeStatus([FromBody] List<StatusChangeinput> changeModels)
        {
            var date = DateTime.Now;
            changeModels.ForEach(model => {
                var changeitem = new StatusChangeModel {

                    ItemId = model.ItemId,
                    Beforechange = model.Beforechange,
                    StatusCode = model.StatusCode,
                    PlaceId = model.PlaceId,
                    UserId = model.UserId,
                    ChangeDate = date
                };
                var fireitem = Context.FireitemsTable.Where(y => y.ItemId.Equals(model.ItemId)).FirstOrDefault();
                fireitem.PresentStatus = model.StatusCode;

                Context.StatusChangeTable.Add(changeitem);

            });

            int count = await Context.SaveChangesAsync();

            return Ok(new { total = count });
        }

        [HttpGet]
        [Authorize("API")]
        public IActionResult ChangeStatusRecord()
        {
            var record = Context.PlaceTable.Include(x => x.StatusChangeList).ThenInclude(x => x.FireitemRef);

            return Ok(record);
        }

        [HttpPost]
        [Authorize("API")]
        public async Task<IActionResult> Inventory([FromBody] InventoryInput input)
        {


            var record = new InventoryEventModel {

                UserId = input.UserId,
                PlaceId = input.PlaceId,
                InventoryDate = DateTime.Now,

                InventoryItemList = input.InventoryItemList

            };


            Context.InventoryEventTable.Add(record);
            int inventorycount = await Context.SaveChangesAsync() - 1;

            input.InventoryItemList.Where(x => x.StatusAfter != x.StatusBefore).ToList().ForEach(
                x => {


                    var fireitem = Context.FireitemsTable.Where(y => y.ItemId.Equals(x.ItemId)).FirstOrDefault();
                    fireitem.PresentStatus = x.StatusAfter;

                    Context.StatusChangeTable.Add(new StatusChangeModel {

                        UserId = input.UserId,
                        ItemId = x.ItemId,
                        Beforechange = x.StatusBefore,
                        StatusCode = x.StatusAfter,
                        PlaceId = input.PlaceId,
                        ChangeDate = DateTime.Now
                    }); ;
                }
                );

            int statuschangecount = await Context.SaveChangesAsync();




            return Ok(new { inventory = inventorycount, changecount = statuschangecount });
        }

        [Authorize("API")]
        public IActionResult Inventoryrecord()
        {


            var record = Context.PlaceTable.Include(x => x.InventoryEventList);

            return Ok(record);

        }
        [Authorize("API")]
        public IActionResult InventoryItemrecord(int inventoryid)
        {


            var record = Context.InventoryItemsTable.Where(x => x.EventId == inventoryid).Include(x => x.FireitemsRef).ThenInclude(x => x.PriorityRef);

            return Ok(record);

        }






        [HttpPost]
        [Authorize("API")]
        public async Task<IActionResult> additem([FromBody] List< Fireiteminput> input)
        {
            input.ForEach(item => {

                var fireitem = new FireitemModel {
                    ItemId = DateTime.Now.ToString("yyyyMMddHHmmssfff" + input.IndexOf(item)),
                    ItemName = item.ItemName,
                    postscript = item.postscript,
                    PresentStatus = 0,
                    InventoryStatus = 5,
                    StoreId = item.StoreId
                };
                Context.FireitemsTable.Add(fireitem);
            });

          
          
            int count = await Context.SaveChangesAsync();
            return Ok("成功新增"+ count+"項設備");


        }

        [HttpGet]
        [Authorize("API")]
        public IActionResult Placeinfo()
        {

            var placeinfo = Context.PlaceTable.Include(x => x.PriorityList);

            return Ok(placeinfo);

        }

        [HttpGet]
        [Authorize("API")]
        public async Task<IActionResult> generatecode()
        {
            var items = Context.PlaceTable.Include(x => x.PriorityList).ThenInclude(x => x.FireitemList).ToList();


            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Barcode";
            try {
                Directory.CreateDirectory(desktop);

                //Directory.CreateDirectory(desktop + @"\BarcodeWord");


            }
            catch (Exception ex) {

            }


            BarcodeWriter barcodeWriter = new BarcodeWriter {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions {
                    Height = 40,
                    Width = 100,
                    PureBarcode = true,
                    Margin = 0
                }
            };


            items.ForEach(place => {
                try {
                    Directory.CreateDirectory(desktop + @"\" + place.PlaceName);

                }
                catch (Exception ex) {

                }
                var alllist = place.PriorityList.Select(x => x.FireitemList).Aggregate((a, b) => (a.Concat(b).ToList()));
                int row = alllist.Count % 3 == 0 ? alllist.Count / 3 : alllist.Count / 3 + 1;
                Document Barcodedoc = new Document();
                Table Barcodetable = Barcodedoc.AddSection().AddTable(true);
                Barcodetable.ResetCells(row, 3);


                place.PriorityList.ToList().ForEach(priority => {

                    Directory.CreateDirectory(desktop + @"\" + place.PlaceName + @"\" + priority.SubArea);


                    priority.FireitemList.ToList().ForEach(fireitem => {
                        Bitmap fireitembarcode = barcodeWriter.Write(fireitem.ItemId);
                        fireitembarcode.Save(desktop + @"\" + place.PlaceName + @"\" + priority.SubArea + @"\" + fireitem.ItemId + ".png", ImageFormat.Png);




                        int index = alllist.IndexOf(fireitem);

                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TextRange TR = Barcodetable[index / 3, index % 3].Paragraphs[0].AppendText(fireitem.ItemId + "(" + fireitem.ItemName + ")");
                        TR.CharacterFormat.FontSize = 8;




                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[1].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        DocPicture picture = Barcodetable[index / 3, index % 3].Paragraphs[1].AppendPicture(fireitembarcode);

                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[2].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR = Barcodetable[index / 3, index % 3].Paragraphs[2].AppendText(place.PlaceName + "(" + priority.SubArea + ")");

                        TR.CharacterFormat.FontSize = 8;
                    });





                });

                Barcodedoc.SaveToFile(desktop + @"\" + place.PlaceName + @"\" + place.PlaceName + ".docx");


            });


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("hexes128@gmail.com"));
            message.To.Add(new MailboxAddress("hexes128@gmail.com"));
            message.Subject = "盤點Barcode";

            var builder = new BodyBuilder();




            items.ForEach(place => {
                builder.Attachments.Add(desktop + @"\" + place.PlaceName + @"\" + place.PlaceName + ".docx");
            });
            message.Body = builder.ToMessageBody();
            await provider.SmtpClient.SendAsync(message);





            return Ok("");

        }


        [HttpGet]
        //[Authorize("API")]
        public async Task<IActionResult> generatecodewithoutsave(string email)
        {
            
            
            var items = Context.PlaceTable.Include(x => x.PriorityList).ThenInclude(x => x.FireitemList).ToList();

        
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Barcode";
            try {
                Directory.CreateDirectory(desktop);




            }
            catch (Exception ex) {
                return Ok(ex.Message);
            }


            BarcodeWriter barcodeWriter = new BarcodeWriter {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions {
                    Height = 40,
                    Width = 100,
                    PureBarcode = true,
                    Margin = 0
                }
            };


            items.ForEach(place => {

                var alllist = place.PriorityList.Select(x => x.FireitemList).Aggregate((a, b) => (a.Concat(b).ToList()));
                if (alllist.Count == 0) {
                    return;
                }
                int row = alllist.Count % 3 == 0 ? alllist.Count / 3 : alllist.Count / 3 + 1;
                Document Barcodedoc = new Document();
                Table Barcodetable = Barcodedoc.AddSection().AddTable(true);
                Barcodetable.ResetCells(row, 3);


                place.PriorityList.ToList().ForEach(priority => {




                    priority.FireitemList.ToList().ForEach(fireitem => {
                        Bitmap fireitembarcode = barcodeWriter.Write(fireitem.ItemId);



                        int index = alllist.IndexOf(fireitem);

                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TextRange TR = Barcodetable[index / 3, index % 3].Paragraphs[0].AppendText(fireitem.ItemId + "(" + fireitem.ItemName + ")");
                        TR.CharacterFormat.FontSize = 8;




                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[1].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        DocPicture picture = Barcodetable[index / 3, index % 3].Paragraphs[1].AppendPicture(fireitembarcode);

                        Barcodetable[index / 3, index % 3].AddParagraph();
                        Barcodetable[index / 3, index % 3].Paragraphs[2].Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TR = Barcodetable[index / 3, index % 3].Paragraphs[2].AppendText(place.PlaceName + "(" + priority.SubArea + ")");

                        TR.CharacterFormat.FontSize = 8;
                    });





                });

                Barcodedoc.SaveToFile(desktop + @"\" + place.PlaceName + ".docx");


            });


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("hexes128@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "盤點Barcode";

            var builder = new BodyBuilder();




            items.ForEach(place => {
                if (System.IO.File.Exists(desktop + @"\" + place.PlaceName + ".docx")) {
                    builder.Attachments.Add(desktop + @"\" + place.PlaceName + ".docx");
                }



            });
            message.Body = builder.ToMessageBody();
            await provider.SmtpClient.SendAsync(message);





            return Ok(desktop);

        }


        [HttpPost]
        [Authorize("API")]
        public async Task<IActionResult> editinfo([FromBody] editinforecord input)
        {

            var fireitem = Context.FireitemsTable.Find(input.itemid);

            fireitem.ItemName = input.newname;
            fireitem.StoreId = input.newstore;

            int edit =  await Context.SaveChangesAsync();

            input.ChangeDate = DateTime.Now;

            Context.EditinforecordTable.Add(input);


            int editrecord = await Context.SaveChangesAsync();

            int inventorydelete = 0;
            int statusdelete = 0;
            var oldplaceid = Context.PriorityTable.Single(e => e.StoreId == input.oldstore).PlaceId;
            var newplaceid = Context.PriorityTable.Single(e => e.StoreId == input.newstore).PlaceId;
            if (oldplaceid != newplaceid) {


                var inventoryrecord = Context.InventoryItemsTable.Where(x => x.ItemId.Equals(fireitem.ItemId)).ToList();
                inventoryrecord.ForEach(x => Context.InventoryItemsTable.Remove(x));
                inventorydelete = await Context.SaveChangesAsync();

                var statusrecord = Context.StatusChangeTable.Where(x => x.ItemId.Equals(fireitem.ItemId)).ToList();

                statusrecord.ForEach(x => Context.StatusChangeTable.Remove(x));

                statusdelete = await Context.SaveChangesAsync();
            }


            return Ok("編輯" + edit + "盤點紀錄" + inventorydelete + "狀態" + statusdelete);
        }


        [HttpPost]
        [Authorize("API")]
        public async Task<IActionResult> addplace([FromBody] PlaceModel input)
        {

            var placename = Context.PlaceTable.Where(x => x.PlaceName.Equals(input.PlaceName)).FirstOrDefault();

            if (placename != null) {
                return Ok("此地點名稱已存在");
            }
            var place = new PlaceModel {
                PlaceName = input.PlaceName,

                todaysend = false,

                PriorityList = input.PriorityList
            };


            Context.PlaceTable.Add(place);

            int count = await Context.SaveChangesAsync();
            if (count > 0) {
                return Ok("成功新增");
            }
            else {
                return Ok("新增失敗 請重新傳送");
            }

         

        }

        [HttpGet]
        [Authorize("API")]
        public IActionResult editinforecord()
        {
            var placeinfo = from x in Context.PriorityTable
                         join
                            y in Context.PlaceTable
                                    on new { x.PlaceId }
                                   equals new { y.PlaceId }
                         select new { x.StoreId,y.PlaceName, x.SubArea };


            var record = from x in Context.EditinforecordTable
                         join y in placeinfo
                         on new { con = x.oldstore }
                         equals
                         new { con = y.StoreId }
                         join z in placeinfo
                         on new { con = x.newstore }
                         equals new { con = z.StoreId }

                         select new { x.itemid, x.oldname, x.newname,oldplace = y.PlaceName,newplace= z.PlaceName,oldarea = z.SubArea,newarea= z.SubArea ,x.ChangeDate,x.UserId};



            return Ok(record);



        }
    }
}
