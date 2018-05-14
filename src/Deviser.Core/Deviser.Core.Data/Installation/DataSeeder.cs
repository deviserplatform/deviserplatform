using System;
using System.Collections.Generic;
using System.Text;
using Deviser.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Deviser.Core.Data.Installation
{
    public class DataSeeder
    {
        private readonly DeviserDbContext _dbContext;

        public DataSeeder(DeviserDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public void InsertData()
        {
            //ContentType
            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("817fea8f-59e2-4b77-8e63-1ea002772893"),

                IconClass = "fa fa-check-square-o",

                IconImage = null,

                Label = "Features",

                Name = "features",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IconClass = "fa fa-code",

                IconImage = null,

                Label = "Rich Text",

                Name = "richtext",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("f99a54f8-5704-4bc1-b287-3a930c9ece53"),

                IconClass = "fa fa-picture-o",

                IconImage = null,

                Label = "Gallery",

                Name = "gallery",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("ad98e4c6-204e-492c-b1d9-48cca97e2cbf"),

                IconClass = null,

                IconImage = null,

                Label = "test",

                Name = "test",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("c49840f4-5a00-4d1d-86b7-7881e3841314"),

                IconClass = "fa fa-users",

                IconImage = null,

                Label = "Team",

                Name = "team",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("978bd890-7dbd-4ee0-9d86-8356dfadf4e6"),

                IconClass = "fa fa-clone",

                IconImage = null,

                Label = "Teaser Box",

                Name = "teaserbox",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                IconClass = "fa fa-video-camera",

                IconImage = null,

                Label = "Video",

                Name = "video",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("69933d62-31ed-481e-be1f-95dfb8210027"),

                IconClass = "fa fa-external-link",

                IconImage = null,

                Label = "Call to action",

                Name = "calltoaction",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("f2e91a21-0864-4b16-b3de-9be08888b91f"),

                IconClass = "fa fa-picture-o",

                IconImage = null,

                Label = "Image",

                Name = "image",

                SortOrder = 2,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("a3e319ea-80b9-4800-9032-bb7ea09ed331"),

                IconClass = "fa fa-rss-square",

                IconImage = null,

                Label = "Blog",

                Name = "blog",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("8d878db7-c3e2-4c39-b359-bd0e39d87df9"),

                IconClass = "fa fa-folder-o",

                IconImage = null,

                Label = "Tabs",

                Name = "tabs",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("00332002-f2c7-401c-b59c-d0181eaf657b"),

                IconClass = "fa fa-font",

                IconImage = null,

                Label = "Text",

                Name = "text",

                SortOrder = 1,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("b2c35761-a953-4bf7-bfb2-d0ea9e63786d"),

                IconClass = "fa fa-map-o",

                IconImage = null,

                Label = "Map",

                Name = "map",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IconClass = "fa fa-bars",

                IconImage = null,

                Label = "Accordion",

                Name = "accordion",

                SortOrder = 0,
            });

            _dbContext.Set<ContentType>
            ().Add(new ContentType
            {

                Id = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IconClass = "fa fa-caret-square-o-right",

                IconImage = null,

                Label = "Slider",

                Name = "slider",

                SortOrder = 3,
            });

            //Language

            _dbContext.Set<Language>
            ().Add(new Language
            {

                Id = Guid.Parse("388b34ed-803a-48ad-6618-08d360bcd031"),

                CultureCode = "de-CH",

                EnglishName = "German (Switzerland)",

                FallbackCulture = "en-US",

                NativeName = "Deutsch (Schweiz)",
            });

            _dbContext.Set<Language>
            ().Add(new Language
            {

                Id = Guid.Parse("1350c0b2-634e-4e81-6619-08d360bcd031"),

                CultureCode = "fr-CH",

                EnglishName = "French (Switzerland)",

                FallbackCulture = "en-US",

                NativeName = "français (Suisse)",
            });

            _dbContext.Set<Language>
            ().Add(new Language
            {

                Id = Guid.Parse("d17b0b1f-129c-4bd8-1848-08d3f060a575"),

                CultureCode = "ta-IN",

                EnglishName = "Tamil (India)",

                FallbackCulture = "en-US",

                NativeName = "தமிழ் (இந்தியா)",
            });

            _dbContext.Set<Language>
            ().Add(new Language
            {

                Id = Guid.Parse("4a8a96c4-b125-433a-b0b7-e8ddbcfaa381"),

                CultureCode = "en-US",

                EnglishName = "English (United States)",

                FallbackCulture = "en-US",

                NativeName = "English (United States)",
            });

            //Layout

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("c4843750-1e7c-45f9-bf11-08d3a4535e73"),

                Config = "[{\"Id\":\"0fcf04a2-3d71-26b0-c371-6d936c6c65d8\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]",

                IsDeleted = false,

                Name = "Contact Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("65e472d9-4ea7-4aaf-278d-08d536a7eb6b"),

                Config = "[{\"Id\":\"bc67cef1-a571-ad77-93e3-fab7a0d212b2\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]},{\"Id\":\"c2245c7d-45d1-0aac-0855-1ae86b001c95\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":2,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"f4d81c80-712b-0c08-0f23-0a8e3fa03fe4\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e75bcaff-78fe-4250-323a-9c7f30786548\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":\"col-md-3\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width \",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"}],\"PlaceHolders\":[]}]},{\"Id\":\"17bb9a8f-8e26-e822-b37d-26b1acfa6757\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":2,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"7bb4d1af-616f-cc0b-00e5-a2564a6bfe20\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":\"col-md-6\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width \",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"}],\"PlaceHolders\":[]},{\"Id\":\"13d6a3af-a95f-d02d-d312-9c41ed6e9f74\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":2,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":\"col-md-6\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width \",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"}],\"PlaceHolders\":[]}]},{\"Id\":\"2bc2c9cb-2513-b84e-d2fe-2878226a3acf\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":3,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"d01c2312-53bb-3c2a-e437-e3b58c3e07dd\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":\"col-md-6\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width \",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"}],\"PlaceHolders\":[]}]},{\"Id\":\"790e00c4-175f-eae1-92c4-9f55bc09d73a\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":4,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"column_width\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]",

                IsDeleted = false,

                Name = "Test Copy",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("fc147e43-55db-4a09-5f08-08d5724050a8"),

                Config = "[{\"Id\":\"ae8e2533-099d-2b82-20a3-7aea76171889\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]",

                IsDeleted = false,

                Name = "test",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("bef258d0-173c-4b70-343c-08d58851a063"),

                Config = "[{\"Id\":\"4cbbdba7-8bfa-04b0-6922-d1bc269fb28f\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"6c61bf58-3dd1-a9c7-db96-c31bec104b62\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]},{\"Id\":\"da9532c1-2e4e-38ef-9634-ce54a6796fba\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":2,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"white-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"11206d0e-1883-b896-a733-b33ee43afc05\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"1b8d8a32-b6fc-db24-4640-e6372ced1bf2\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"0e017a7e-f347-af39-1319-78e9f2bc46eb\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":\"055c1a23-1fd5-a10e-e01c-46462461c438\",\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":\"dfb3073f-f4e4-d6c6-9b67-c192a575597e\",\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":\"47342187-08af-37d2-cb6b-17f051233f52\",\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":\"237b37d3-2711-afe7-5f2d-e26556658719\",\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]}]}]}]},{\"Id\":\"0523a311-06d0-23cb-698a-dc88014782c5\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":3,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"grey-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"2a59cc4a-bc3a-83f8-fe81-d22f0135a344\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e30ab8bf-25d2-8962-3356-924b5b243dec\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"45c16d69-788e-b85e-c4b8-3a9d3b9d7d7d\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"22a46d38-7cad-0921-fd30-3a40b2933575\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]},{\"Id\":\"83049e6c-9d21-f5b9-4158-b4dd4aae4b34\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":2,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"22a46d38-7cad-0921-fd30-3a40b2933575\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]}]}]}]",

                IsDeleted = false,

                Name = "Demo Home",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("04e4fd47-d326-400f-31c1-08d59356951e"),

                Config = "[{\"Id\":\"d40fbf76-215d-b78e-4bfd-4216b04682a0\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"0e2641ad-c6bc-9811-bdca-9396be4a6b1a\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"container page-title\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"9f9be029-8a37-de11-dd6b-b31146fbfddb\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"abe9b84e-c757-885b-8831-eb13ca2350f6\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]},{\"Id\":\"25e41b6f-8809-f983-edde-0516a89feeb2\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":2,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"container\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]",

                IsDeleted = false,

                Name = "Demo Gallery Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("f51483cb-bb13-4632-f374-08d5a2c90b56"),

                Config = "[{\"Id\":\"61715f60-dcec-0d8c-e8ff-87ebd526a768\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"ff32cfe7-c0d0-4cb0-6a95-37e5b651d847\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e75e2f92-a360-57e4-1415-ae18ddd71158\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"white-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"77ac7c1a-b31a-0a4c-72a8-aa563c318288\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"a43f079d-794a-abaa-67c9-ab4835006d7a\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]}]}]}]",

                IsDeleted = false,

                Name = "Demo Tabs",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("68c349b1-dcc9-47ef-f375-08d5a2c90b56"),

                Config = "[{\"Id\":\"61715f60-dcec-0d8c-e8ff-87ebd526a768\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"ff32cfe7-c0d0-4cb0-6a95-37e5b651d847\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e75e2f92-a360-57e4-1415-ae18ddd71158\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"white-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"77ac7c1a-b31a-0a4c-72a8-aa563c318288\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"a43f079d-794a-abaa-67c9-ab4835006d7a\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]}]}]}]",

                IsDeleted = false,

                Name = "Demo Accordion",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("aa05d040-361d-45a8-d347-08d5a398fdd9"),

                Config = "[{\"Id\":\"61715f60-dcec-0d8c-e8ff-87ebd526a768\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"a8bb3a4f-d4a0-94bb-c1d2-6d1734074a78\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"page-title\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"24464d4d-9422-2ed7-2fdc-c8e3bb96f316\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"40531ef2-a872-a56a-c06e-8f4bd93cce53\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]},{\"Id\":\"ff32cfe7-c0d0-4cb0-6a95-37e5b651d847\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":2,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e75e2f92-a360-57e4-1415-ae18ddd71158\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"77ac7c1a-b31a-0a4c-72a8-aa563c318288\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]}]}]",

                IsDeleted = false,

                Name = "Demo Services",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("7605e49c-da3a-4b25-b67a-08d5a5682b4f"),

                Config = "[{\"Id\":\"61715f60-dcec-0d8c-e8ff-87ebd526a768\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"ff32cfe7-c0d0-4cb0-6a95-37e5b651d847\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"b4f9aa96-b2db-7f04-c70b-8d85bf917875\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"page-title\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"4b6bf9e4-c146-1e9f-481a-25e33f4aa787\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"a4f55be5-c300-45f5-5412-7e8e5c73b0ae\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]},{\"Id\":\"e75e2f92-a360-57e4-1415-ae18ddd71158\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":2,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"white-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"77ac7c1a-b31a-0a4c-72a8-aa563c318288\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"ff1a557e-662e-d6c1-c124-65ae939cd243\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]}]}]}]",

                IsDeleted = false,

                Name = "Team Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("49ca6d71-a647-4b3e-8c59-08d5b45b8cf4"),

                Config = "[{\"Id\":\"1f95a2c2-abf2-cb3b-7dc5-8449e7e8a220\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"container\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"26e7b693-7dff-d941-4492-b313b05fef6e\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]}]",

                IsDeleted = false,

                Name = "Blog Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("af7533aa-3e4b-4e29-b118-1841ab5a0a91"),

                Config = "[{\"Id\":\"06aab8db-4118-f83c-e9b0-8ee35f1c8212\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"2adfbd0a-dbed-84ee-8c7a-c296a48c483c\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"ere\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"f26bfe84-7ebe-377c-67e3-a889545eaa31\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]}]}]}]",

                IsDeleted = false,

                Name = "Login Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("d6471f27-716c-4f6f-a10b-4acef3fa4da3"),

                Config = "[{\"Id\":\"bc67cef1-a571-ad77-93e3-fab7a0d212b2\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"0aa1c2e1-2bb4-3781-f12d-ce9940bb1171\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"grey-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"c2245c7d-45d1-0aac-0855-1ae86b001c95\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"f4d81c80-712b-0c08-0f23-0a8e3fa03fe4\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"e75bcaff-78fe-4250-323a-9c7f30786548\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"22a46d38-7cad-0921-fd30-3a40b2933575\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[{\"Id\":\"977fac14-8488-53e4-181c-005912ce90e0\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"3b3fe768-5622-a826-643c-3afad26b71af\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2d4c86e6-b85b-e278-7591-906ccc3f8fe5\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]},{\"Id\":\"bc07a679-0c15-6070-98dd-f0815fd36697\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":2,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2d4c86e6-b85b-e278-7591-906ccc3f8fe5\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]}]}]},{\"Id\":\"9e25d404-e1b9-9ea8-9687-0f0bf294cd5e\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":2,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"22a46d38-7cad-0921-fd30-3a40b2933575\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]}]}]}]},{\"Id\":\"6e8d0f95-bde5-4202-6a9f-12b355f34c00\",\"Type\":\"section\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":2,\"LayoutTypeId\":\"a72ce8d2-5f5f-4176-0141-08d57ef792d3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"white-section\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"0138cd4d-2d9b-fb94-72c4-5fe3335c20dd\",\"Type\":\"container\",\"LayoutTemplate\":\"container\",\"SortOrder\":1,\"LayoutTypeId\":\"9341f92e-83d8-4afe-ad4a-a95deeda9ae3\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"3438d12b-a25d-bb5e-d974-cea403d355c4\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":1,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"c7d1df63-d43c-9f98-2f63-22b48f1b4735\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"},{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]},{\"Id\":\"17bb9a8f-8e26-e822-b37d-26b1acfa6757\",\"Type\":\"row\",\"LayoutTemplate\":\"row\",\"SortOrder\":2,\"LayoutTypeId\":\"43734210-943e-4f33-a161-f12260b8c001\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[{\"Id\":\"7bb4d1af-616f-cc0b-00e5-a2564a6bfe20\",\"Type\":\"column\",\"LayoutTemplate\":\"column\",\"SortOrder\":1,\"LayoutTypeId\":\"4c98f160-d676-40a2-9b88-79fd1343f333\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":\"\",\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":null,\"LastModifiedDate\":null},{\"Id\":\"f4fff310-340a-437a-8ce3-08d54de42fea\",\"Name\":\"column_width\",\"Label\":\"Column Width (M)\",\"Value\":\"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\"DefaultValue\":\"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\"Description\":\"Column width for extra small (XS) devices: ≥768px\",\"OptionListId\":\"969eb4ef-188c-4174-4194-08d54de4cd18\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2017-12-28T12:14:39.4480249\",\"LastModifiedDate\":\"2017-12-28T12:14:39.4480249\"},{\"Id\":\"3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05\",\"Name\":\"column_width_xs\",\"Label\":\"Column Width (XS)\",\"Value\":null,\"DefaultValue\":\"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\"Description\":\"Column width for extra small (XS) devices: <576px\",\"OptionListId\":\"b98ba240-64b4-4862-f1fb-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:09:46.2963207\",\"LastModifiedDate\":\"2018-03-18T00:09:46.2963207\"},{\"Id\":\"3379dca0-ada6-4245-aeab-08d58c5c2d05\",\"Name\":\"column_width_s\",\"Label\":\"Column Width (S)\",\"Value\":null,\"DefaultValue\":\"e7cf0d7d-e66b-09cb-f338-720588431bee\",\"Description\":\"Colum width for Small (S) devices: ≥576px\",\"OptionListId\":\"17fa4063-c7b4-4659-f1fc-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:10:49.9721966\",\"LastModifiedDate\":\"2018-03-18T00:10:49.9721966\"},{\"Id\":\"14539266-bd68-440f-aeac-08d58c5c2d05\",\"Name\":\"column_width_l\",\"Label\":\"Column Width (L)\",\"Value\":null,\"DefaultValue\":\"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\"Description\":\"Column width for Large (L) devices: ≥768px\",\"OptionListId\":\"ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:12:06.3393526\",\"LastModifiedDate\":\"2018-03-18T00:12:06.3393526\"},{\"Id\":\"f0ad85a3-ff8c-40c6-aead-08d58c5c2d05\",\"Name\":\"column_width_xl\",\"Label\":\"Column Width (XL)\",\"Value\":null,\"DefaultValue\":\"4e3ff987-b14b-0713-4243-053fc5787389\",\"Description\":\"Column width for Extra Large (XL) devices: ≥1200px\",\"OptionListId\":\"bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88\",\"OptionList\":null,\"IsActive\":true,\"CreatedDate\":\"2018-03-18T00:13:00.1266679\",\"LastModifiedDate\":\"2018-03-18T00:13:00.1266679\"}],\"PlaceHolders\":[]}]}]}]}]}]",

                IsDeleted = false,

                Name = "Home Page Layout",
            });

            _dbContext.Set<Layout>
            ().Add(new Layout
            {

                Id = Guid.Parse("af8ecc7d-e300-41af-b55a-deeb097836d2"),

                Config = "[{\"Id\":\"a2b3cf83-2533-27f9-b8fc-843681daa777\",\"Type\":\"wrapper\",\"LayoutTemplate\":\"repeater\",\"SortOrder\":1,\"LayoutTypeId\":\"5a0a5884-da84-4922-a02f-5828b55d5c92\",\"Properties\":[{\"Id\":\"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\"Name\":\"css_class\",\"Label\":\"Css Class\",\"Value\":null,\"DefaultValue\":null,\"Description\":null,\"OptionListId\":null,\"OptionList\":null,\"IsActive\":false,\"CreatedDate\":null,\"LastModifiedDate\":null}],\"PlaceHolders\":[]}]",

                IsDeleted = false,

                Name = "Admin Layout",
            });

            //LayoutType

            _dbContext.Set<LayoutType>
            ().Add(new LayoutType
            {

                Id = Guid.Parse("a72ce8d2-5f5f-4176-0141-08d57ef792d3"),

                IconClass = "fa fa-code",

                IconImage = null,

                Label = "Section",

                LayoutTypeIds = "9341f92e-83d8-4afe-ad4a-a95deeda9ae3,5a0a5884-da84-4922-a02f-5828b55d5c92,43734210-943e-4f33-a161-f12260b8c001",

                Name = "section",
            });

            _dbContext.Set<LayoutType>
            ().Add(new LayoutType
            {

                Id = Guid.Parse("5a0a5884-da84-4922-a02f-5828b55d5c92"),

                IconClass = "fa fa-square-o",

                IconImage = null,

                Label = "Wrapper",

                LayoutTypeIds = "9341f92e-83d8-4afe-ad4a-a95deeda9ae3, 43734210-943e-4f33-a161-f12260b8c001",

                Name = "wrapper",
            });

            _dbContext.Set<LayoutType>
            ().Add(new LayoutType
            {

                Id = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                IconClass = "fa fa-columns",

                IconImage = null,

                Label = "Column",

                LayoutTypeIds = "9341f92e-83d8-4afe-ad4a-a95deeda9ae3,43734210-943e-4f33-a161-f12260b8c001",

                Name = "column",
            });

            _dbContext.Set<LayoutType>
            ().Add(new LayoutType
            {

                Id = Guid.Parse("9341f92e-83d8-4afe-ad4a-a95deeda9ae3"),

                IconClass = "fa fa-square-o",

                IconImage = null,

                Label = "Container",

                LayoutTypeIds = "9341f92e-83d8-4afe-ad4a-a95deeda9ae3, 43734210-943e-4f33-a161-f12260b8c001",

                Name = "container",
            });

            _dbContext.Set<LayoutType>
            ().Add(new LayoutType
            {

                Id = Guid.Parse("43734210-943e-4f33-a161-f12260b8c001"),

                IconClass = "fa fa-align-justify",

                IconImage = null,

                Label = "Row",

                LayoutTypeIds = "4c98f160-d676-40a2-9b88-79fd1343f333",

                Name = "row",
            });

            //Module

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("d670ac96-2ab6-4036-4664-08d52acdf1a1"),

                Description = "Recycle Bin",

                Label = "RecycleBin",

                Name = "RecycleBin",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                Description = "Contact",

                Label = "Contact",

                Name = "Contact",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("654f660d-9c71-48f9-8237-593a39a0dbc0"),

                Description = "Security Roles",

                Label = "Security Roles",

                Name = "SecurityRoles",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),

                Description = "Security Module",

                Label = "Security Module",

                Name = "Security",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("e99086da-297e-4fdd-a84c-74c663baf9ae"),

                Description = "Site Management",

                Label = "Site Management",

                Name = "SiteManagement",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("f07dbddf-4937-42b8-9bee-9c0713128013"),

                Description = "File Management",

                Label = "File Management",

                Name = "FileManager",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("0c30609d-87f3-4d84-9269-cfba91e5c0b6"),

                Description = "User Management",

                Label = "User Management",

                Name = "UserManagement",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),

                Description = "Content Management",

                Label = "Content Management",

                Name = "ContentManagement",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("f271f063-aa57-4ee0-95a4-d1417fab15c4"),

                Description = "Module Management",

                Label = "Module Management",

                Name = "ModuleManagement",

                Version = "00.00.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("57813091-da9f-47e3-9d63-dd5c4df79f1d"),

                Description = "Page Manag§ement",

                Label = "Page Management",

                Name = "PageManagement",

                Version = "00.01.00",
            });

            _dbContext.Set<Module>
            ().Add(new Module
            {

                Id = Guid.Parse("73829a91-4a4a-4c22-885a-fb1215e37fdc"),

                Description = "Language",

                Label = "Language",

                Name = "Language",

                Version = "00.01.00",
            });

            //ModuleActionType

            _dbContext.Set<ModuleActionType>
            ().Add(new ModuleActionType
            {

                Id = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ControlType = "View",
            });

            _dbContext.Set<ModuleActionType>
            ().Add(new ModuleActionType
            {

                Id = Guid.Parse("192278b6-7bf2-40c2-a776-b9ca5fb04fbb"),

                ControlType = "Edit",
            });


            //OptionList
            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("969eb4ef-188c-4174-4194-08d54de4cd18"),

                Label = "Column Width (Medium)",

                List = "[\r\n  {\r\n    \"id\": \"b211626d-09e3-c23a-e731-7cd85d33871a\",\r\n    \"name\": \"col-md-1\",\r\n    \"label\": \"col-md-1\"\r\n  },\r\n  {\r\n    \"id\": \"cc70e2c9-2af8-cf7c-9596-8af075c626e6\",\r\n    \"name\": \"col-md-2\",\r\n    \"label\": \"col-md-2\"\r\n  },\r\n  {\r\n    \"id\": \"2d4c86e6-b85b-e278-7591-906ccc3f8fe5\",\r\n    \"name\": \"col-md-3\",\r\n    \"label\": \"col-md-3\"\r\n  },\r\n  {\r\n    \"id\": \"f2eafdde-4f79-a195-c1c1-0794e293fa27\",\r\n    \"name\": \"col-md-4\",\r\n    \"label\": \"col-md-4\"\r\n  },\r\n  {\r\n    \"id\": \"48474435-0004-8fa7-33d2-f1b5de5ecaff\",\r\n    \"name\": \"col-md-5\",\r\n    \"label\": \"col-md-5\"\r\n  },\r\n  {\r\n    \"id\": \"22a46d38-7cad-0921-fd30-3a40b2933575\",\r\n    \"name\": \"col-md-6\",\r\n    \"label\": \"col-md-6\"\r\n  },\r\n  {\r\n    \"id\": \"7fd1245f-c218-7a56-f388-7f41a051a81a\",\r\n    \"name\": \"col-md-7\",\r\n    \"label\": \"col-md-7\"\r\n  },\r\n  {\r\n    \"id\": \"1bc35e70-9f0e-4038-e182-ebdfac4ea653\",\r\n    \"name\": \"col-md-8\",\r\n    \"label\": \"col-md-8\"\r\n  },\r\n  {\r\n    \"id\": \"65745a21-9d59-3b95-50fd-39586f61957a\",\r\n    \"name\": \"col-md-9\",\r\n    \"label\": \"col-md-9\"\r\n  },\r\n  {\r\n    \"id\": \"f8f8ff2e-88b0-f66a-20cf-07c3339424cf\",\r\n    \"name\": \"col-md-10\",\r\n    \"label\": \"col-md-10\"\r\n  },\r\n  {\r\n    \"id\": \"3d41a705-ca67-2510-1d09-967419377e55\",\r\n    \"name\": \"col-md-11\",\r\n    \"label\": \"col-md-11\"\r\n  },\r\n  {\r\n    \"id\": \"2c6d4b41-a103-d2a0-4d2f-0b75646d05fe\",\r\n    \"name\": \"col-md-12\",\r\n    \"label\": \"col-md-12\"\r\n  }\r\n]",

                Name = "column_width",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("7893ac90-9793-4c77-f5a2-08d582dfc823"),

                Label = "Video Button",

                List = "[\r\n  {\r\n    \"id\": \"a935f434-6bb4-5c6a-2889-1216180770bf\",\r\n    \"name\": \"preview\",\r\n    \"label\": \"Preview\"\r\n  },\r\n  {\r\n    \"id\": \"71b59001-99b1-7f78-8ce8-f9c3765434e2\",\r\n    \"name\": \"nopreview\",\r\n    \"label\": \"No Preview\"\r\n  }\r\n]",

                Name = "video_button",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("10506766-7323-401c-29b6-08d58b87bb48"),

                Label = "Boolean",

                List = "[\r\n  {\r\n    \"id\": \"20e770b7-b245-16e6-925c-1ce3a036d1ae\",\r\n    \"name\": \"true\",\r\n    \"label\": \"true\"\r\n  },\r\n  {\r\n    \"id\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"name\": \"false\",\r\n    \"label\": \"false\"\r\n  }\r\n]",

                Name = "bool",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("bfae06f9-d850-486c-29b7-08d58b87bb48"),

                Label = "Swiper Direction",

                List = "[\r\n  {\r\n    \"id\": \"954d54e1-5c95-da20-422e-9c31691631b2\",\r\n    \"name\": \"horizontal\",\r\n    \"label\": \"horizontal\"\r\n  },\r\n  {\r\n    \"id\": \"443cfb14-0fc0-6f52-f941-bab9803592d4\",\r\n    \"name\": \"vertical\",\r\n    \"label\": \"vertical\"\r\n  }\r\n]",

                Name = "swiper_direction",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("58b22ebe-cb20-4a43-4f77-08d58be54a36"),

                Label = "Swiper Effect",

                List = "[\r\n  {\r\n    \"id\": \"196f5ee0-c955-7d7a-618d-27863e379a19\",\r\n    \"name\": \"slider\",\r\n    \"label\": \"Slide\"\r\n  },\r\n  {\r\n    \"id\": \"25734e5e-4fd3-6dd8-0f7f-0e9d86354109\",\r\n    \"name\": \"fade\",\r\n    \"label\": \"Fade\"\r\n  },\r\n  {\r\n    \"id\": \"d506ac22-604f-4da1-50c1-d2cfd67f17a3\",\r\n    \"name\": \"cube\",\r\n    \"label\": \"Cube\"\r\n  },\r\n  {\r\n    \"id\": \"e6f8dc77-70da-2af8-00fc-b6be408993f1\",\r\n    \"name\": \"coverflow\",\r\n    \"label\": \"Cover Flow\"\r\n  },\r\n  {\r\n    \"id\": \"e0da701b-ff78-69ba-f033-d88bf0d2b577\",\r\n    \"name\": \"flip\",\r\n    \"label\": \"Flip\"\r\n  }\r\n]",

                Name = "swiper_effect",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("dd48a020-bba4-43e3-4f78-08d58be54a36"),

                Label = "Swiper slides per column fill",

                List = "[\r\n  {\r\n    \"id\": \"449cc0c9-52ca-7f58-bb40-f4c876486ffa\",\r\n    \"name\": \"column\",\r\n    \"label\": \"Column\"\r\n  },\r\n  {\r\n    \"id\": \"ca98241d-a655-b684-1659-a1c00d9f02d2\",\r\n    \"name\": \"row\",\r\n    \"label\": \"Row\"\r\n  }\r\n]",

                Name = "swiper_slidesPerColumnFill",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("c21630e0-e691-4412-4f79-08d58be54a36"),

                Label = "Swiper touch events target",

                List = "[\r\n  {\r\n    \"id\": \"9a530193-4a7e-8602-2046-60e97db38507\",\r\n    \"name\": \"container\",\r\n    \"label\": \"Container\"\r\n  },\r\n  {\r\n    \"id\": \"a7a020f5-b565-5cf3-023b-bac23ae3d343\",\r\n    \"name\": \"wrapper\",\r\n    \"label\": \"Wrapper\"\r\n  }\r\n]",

                Name = "swiper_touchEventsTarget",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("e41fa497-9436-475e-f3da-08d58c5519e4"),

                Label = "Swiper Pagination Type",

                List = "[\r\n  {\r\n    \"id\": \"0a50497a-f3d8-53de-932d-2b30a390b125\",\r\n    \"name\": \"bullets\",\r\n    \"label\": \"Bullets\"\r\n  },\r\n  {\r\n    \"id\": \"0ae4ec32-1903-fba5-6438-27767bcc3af6\",\r\n    \"name\": \"fraction\",\r\n    \"label\": \"Fraction\"\r\n  },\r\n  {\r\n    \"id\": \"aa1661ea-ba33-3fef-134f-86c3930813d7\",\r\n    \"name\": \"progressbar\",\r\n    \"label\": \"Progress Bar\"\r\n  },\r\n  {\r\n    \"id\": \"cd918778-02df-baab-2052-7af79303fe87\",\r\n    \"name\": \"custom\",\r\n    \"label\": \"Custom\"\r\n  }\r\n]",

                Name = "swiper_pagination_type",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("b98ba240-64b4-4862-f1fb-08d58c5a4d88"),

                Label = "Column Width (XS)",

                List = "[\r\n  {\r\n    \"id\": \"9dc71c7c-da8d-27d6-4dde-b3c5f7519949\",\r\n    \"name\": \"col-1\",\r\n    \"label\": \"col-1\"\r\n  },\r\n  {\r\n    \"id\": \"3b7e2a54-9bd9-3459-8ddb-5ac9365b503f\",\r\n    \"name\": \"col-2\",\r\n    \"label\": \"col-2\"\r\n  },\r\n  {\r\n    \"id\": \"3d39d589-ca8e-ef44-b94e-201a8d4cfd99\",\r\n    \"name\": \"col-3\",\r\n    \"label\": \"col-3\"\r\n  },\r\n  {\r\n    \"id\": \"6597d3bd-0971-9d73-968b-64ff6e2eabda\",\r\n    \"name\": \"col-4\",\r\n    \"label\": \"col-4\"\r\n  },\r\n  {\r\n    \"id\": \"a9e5fb1c-30da-3d09-1020-c9e4058295b8\",\r\n    \"name\": \"col-5\",\r\n    \"label\": \"col-5\"\r\n  },\r\n  {\r\n    \"id\": \"c624b2b6-1e55-1113-581a-b3aa42487ced\",\r\n    \"name\": \"col-6\",\r\n    \"label\": \"col-6\"\r\n  },\r\n  {\r\n    \"id\": \"6fae6bae-066e-f70e-6cc9-6b27fae3c93e\",\r\n    \"name\": \"col-7\",\r\n    \"label\": \"col-7\"\r\n  },\r\n  {\r\n    \"id\": \"6481a797-347e-ef0f-4196-fb5d6c6ddc05\",\r\n    \"name\": \"col-8\",\r\n    \"label\": \"col-8\"\r\n  },\r\n  {\r\n    \"id\": \"bb48a7b1-420b-f9f5-fff1-69c1ef972e87\",\r\n    \"name\": \"col-9\",\r\n    \"label\": \"col-9\"\r\n  },\r\n  {\r\n    \"id\": \"df92ad01-fdc3-8c38-8683-923074b216ec\",\r\n    \"name\": \"col-10\",\r\n    \"label\": \"col-10\"\r\n  },\r\n  {\r\n    \"id\": \"758d9218-b241-24dd-5f30-3e5fea633ee8\",\r\n    \"name\": \"col-11\",\r\n    \"label\": \"col-11\"\r\n  },\r\n  {\r\n    \"id\": \"055c1a23-1fd5-a10e-e01c-46462461c438\",\r\n    \"name\": \"col-12\",\r\n    \"label\": \"col-12\"\r\n  }\r\n]",

                Name = "column_width_xs",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("17fa4063-c7b4-4659-f1fc-08d58c5a4d88"),

                Label = "Column Width (S)",

                List = "[\r\n  {\r\n    \"id\": \"3f3343df-fc72-a030-6fc6-76e9fe1649f9\",\r\n    \"name\": \"col-sm-1\",\r\n    \"label\": \"col-sm-1\"\r\n  },\r\n  {\r\n    \"id\": \"87654aad-b339-7151-bbb4-29b28258f053\",\r\n    \"name\": \"col-sm-2\",\r\n    \"label\": \"col-sm-2\"\r\n  },\r\n  {\r\n    \"id\": \"5ecf3c99-42b5-a91a-09e6-6d0a18aa7629\",\r\n    \"name\": \"col-sm-3\",\r\n    \"label\": \"col-sm-3\"\r\n  },\r\n  {\r\n    \"id\": \"e7cf0d7d-e66b-09cb-f338-720588431bee\",\r\n    \"name\": \"col-sm-4\",\r\n    \"label\": \"col-sm-4\"\r\n  },\r\n  {\r\n    \"id\": \"58dc823a-4506-5c61-7a9b-776f011c7f1d\",\r\n    \"name\": \"col-sm-5\",\r\n    \"label\": \"col-sm-5\"\r\n  },\r\n  {\r\n    \"id\": \"5990dae2-6180-3aeb-ce5e-fe94c3eb79ba\",\r\n    \"name\": \"col-sm-6\",\r\n    \"label\": \"col-sm-6\"\r\n  },\r\n  {\r\n    \"id\": \"502b1a02-ae52-f6aa-28fc-aa4c61b4adb2\",\r\n    \"name\": \"col-sm-7\",\r\n    \"label\": \"col-sm-7\"\r\n  },\r\n  {\r\n    \"id\": \"e332ab93-e731-2117-befa-73f326055121\",\r\n    \"name\": \"col-sm-8\",\r\n    \"label\": \"col-sm-8\"\r\n  },\r\n  {\r\n    \"id\": \"8004aca9-524c-8192-6eaf-21bdde2cc13e\",\r\n    \"name\": \"col-sm-9\",\r\n    \"label\": \"col-sm-9\"\r\n  },\r\n  {\r\n    \"id\": \"672b8492-04d9-1344-ebae-b4173e6448d9\",\r\n    \"name\": \"col-sm-10\",\r\n    \"label\": \"col-sm-10\"\r\n  },\r\n  {\r\n    \"id\": \"fd499341-8911-eea2-65a9-912ff7c067ee\",\r\n    \"name\": \"col-sm-11\",\r\n    \"label\": \"col-sm-11\"\r\n  },\r\n  {\r\n    \"id\": \"dfb3073f-f4e4-d6c6-9b67-c192a575597e\",\r\n    \"name\": \"col-sm-12\",\r\n    \"label\": \"col-sm-12\"\r\n  }\r\n]",

                Name = "column_width_s",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("ed74aa2d-dd21-42d9-f1fd-08d58c5a4d88"),

                Label = "Column Width (L)",

                List = "[\r\n  {\r\n    \"id\": \"5479ef96-5ffa-ecc0-3a86-081eecc8bc91\",\r\n    \"name\": \"col-lg-2\",\r\n    \"label\": \"col-lg-1\"\r\n  },\r\n  {\r\n    \"id\": \"073864aa-0bbc-c7dd-4b01-21ddd8572f7e\",\r\n    \"name\": \"col-lg-2\",\r\n    \"label\": \"col-lg-2\"\r\n  },\r\n  {\r\n    \"id\": \"97028213-3609-c16d-52cf-6c9dac539a14\",\r\n    \"name\": \"col-lg-3\",\r\n    \"label\": \"col-lg-3\"\r\n  },\r\n  {\r\n    \"id\": \"5913c029-6cd8-352b-eae0-6f3f2e146b01\",\r\n    \"name\": \"col-lg-4\",\r\n    \"label\": \"col-lg-4\"\r\n  },\r\n  {\r\n    \"id\": \"9eaec3ef-93b5-10e5-4f4e-f76bc09d3ac5\",\r\n    \"name\": \"col-lg-5\",\r\n    \"label\": \"col-lg-5\"\r\n  },\r\n  {\r\n    \"id\": \"4c181fc9-af0c-7f9e-f100-44eb3b1dbc9b\",\r\n    \"name\": \"col-lg-6\",\r\n    \"label\": \"col-lg-6\"\r\n  },\r\n  {\r\n    \"id\": \"ff46a563-f2bc-7255-1561-7b3d066f9302\",\r\n    \"name\": \"col-lg-7\",\r\n    \"label\": \"col-lg-7\"\r\n  },\r\n  {\r\n    \"id\": \"94e3134c-534f-d597-ca8f-89324731d451\",\r\n    \"name\": \"col-lg-8\",\r\n    \"label\": \"col-lg-8\"\r\n  },\r\n  {\r\n    \"id\": \"f86f5f12-a662-c919-69f3-adacd235f0d6\",\r\n    \"name\": \"col-lg-9\",\r\n    \"label\": \"col-lg-9\"\r\n  },\r\n  {\r\n    \"id\": \"9c7e5cf0-9cc7-8d89-018d-72e1c62afeee\",\r\n    \"name\": \"col-lg-10\",\r\n    \"label\": \"col-lg-10\"\r\n  },\r\n  {\r\n    \"id\": \"8cb5c74a-bfcd-87d0-a667-0733ea2f8e38\",\r\n    \"name\": \"col-lg-11\",\r\n    \"label\": \"col-lg-11\"\r\n  },\r\n  {\r\n    \"id\": \"47342187-08af-37d2-cb6b-17f051233f52\",\r\n    \"name\": \"col-lg-12\",\r\n    \"label\": \"col-lg-12\"\r\n  }\r\n]",

                Name = "column_width_l",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("bfd219cc-12a0-4d8b-f1fe-08d58c5a4d88"),

                Label = "Column Width (XL)",

                List = "[\r\n  {\r\n    \"id\": \"3495b691-2d3a-1b82-1b33-5e2955232976\",\r\n    \"name\": \"col-xl-1\",\r\n    \"label\": \"col-xl-1\"\r\n  },\r\n  {\r\n    \"id\": \"f0c7089c-2855-efed-9857-f42e1f6c593e\",\r\n    \"name\": \"col-xl-2\",\r\n    \"label\": \"col-xl-2\"\r\n  },\r\n  {\r\n    \"id\": \"57d8d10f-9067-a7a4-602e-78d2f233ac59\",\r\n    \"name\": \"col-xl-3\",\r\n    \"label\": \"col-xl-3\"\r\n  },\r\n  {\r\n    \"id\": \"4e3ff987-b14b-0713-4243-053fc5787389\",\r\n    \"name\": \"col-xl-4\",\r\n    \"label\": \"col-xl-4\"\r\n  },\r\n  {\r\n    \"id\": \"001ee044-e14b-3a12-f8b8-3cc2556aa990\",\r\n    \"name\": \"col-xl-5\",\r\n    \"label\": \"col-xl-5\"\r\n  },\r\n  {\r\n    \"id\": \"7462c1d1-f3dd-5556-977e-2e80e726cef0\",\r\n    \"name\": \"col-xl-6\",\r\n    \"label\": \"col-xl-6\"\r\n  },\r\n  {\r\n    \"id\": \"871929af-d47e-723d-8feb-954795cbe7e0\",\r\n    \"name\": \"col-xl-7\",\r\n    \"label\": \"col-xl-7\"\r\n  },\r\n  {\r\n    \"id\": \"c4bbca0c-2d38-a927-be66-f995da9a14e8\",\r\n    \"name\": \"col-xl-8\",\r\n    \"label\": \"col-xl-8\"\r\n  },\r\n  {\r\n    \"id\": \"6929bd66-04c9-002a-de63-37af8c87421d\",\r\n    \"name\": \"col-xl-9\",\r\n    \"label\": \"col-xl-9\"\r\n  },\r\n  {\r\n    \"id\": \"4f172221-01d7-0df2-8c69-4c5efb19f513\",\r\n    \"name\": \"col-xl-10\",\r\n    \"label\": \"col-xl-10\"\r\n  },\r\n  {\r\n    \"id\": \"529898d2-d53b-2968-80cd-3c5f83f06fea\",\r\n    \"name\": \"col-xl-11\",\r\n    \"label\": \"col-xl-11\"\r\n  },\r\n  {\r\n    \"id\": \"237b37d3-2711-afe7-5f2d-e26556658719\",\r\n    \"name\": \"col-xl-12\",\r\n    \"label\": \"col-xl-12\"\r\n  }\r\n]",

                Name = "column_width_xl",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("973115b0-c453-4aa6-2b46-08d5a94f289b"),

                Label = "CF View Templates",

                List = "[\r\n  {\r\n    \"id\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"name\": \"DefaultForm\",\r\n    \"label\": \"DefaultForm\"\r\n  },\r\n  {\r\n    \"id\": \"b403e966-8cf2-230e-cb1f-c4a715c2c3a3\",\r\n    \"name\": \"SimpleForm\",\r\n    \"label\": \"SimpleForm\"\r\n  }\r\n]",

                Name = "cf_view_templates",
            });

            _dbContext.Set<OptionList>
            ().Add(new OptionList
            {

                Id = Guid.Parse("fb907561-52ff-49de-2b47-08d5a94f289b"),

                Label = "CF Email Templates",

                List = "[\r\n  {\r\n    \"id\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"name\": \"AdminNotification\",\r\n    \"label\": \"AdminNotification\"\r\n  },\r\n  {\r\n    \"id\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"name\": \"ContactNotification\",\r\n    \"label\": \"ContactNotification\"\r\n  }\r\n]",

                Name = "cf_email_templates",
            });

            //Permission

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                Description = "To edit a content",

                Entity = "CONTENT",

                Label = "Edit Content",

                Name = "EDIT",
            });

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                Description = "To edit a page",

                Entity = "PAGE",

                Label = "Edit Page",

                Name = "EDIT",
            });

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                Description = "To edit a module",

                Entity = "MODULE",

                Label = "Edit Module",

                Name = "EDIT",
            });

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                Description = "To view a content",

                Entity = "CONTENT",

                Label = "View Content",

                Name = "VIEW",
            });

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                Description = "To view a module",

                Entity = "MODULE",

                Label = "View Module",

                Name = "VIEW",
            });

            _dbContext.Set<Permission>
            ().Add(new Permission
            {

                Id = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                Description = "To view a page",

                Entity = "PAGE",

                Label = "View Page",

                Name = "VIEW",
            });

            //Role

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("a4f7b1a6-be5d-4ee4-bdaa-08d3d9c45c75"),

                ConcurrencyStamp = "66fa9843-a6e9-4fa2-bb91-43a1bba70d9c",

                Name = "test role1",

                NormalizedName = "TEST ROLE1",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("689b95dd-3b21-474b-390f-08d542243b25"),

                ConcurrencyStamp = "ae93ac45-1168-476e-83cf-4f5179c4e2df",

                Name = "test role 3",

                NormalizedName = "TEST ROLE 3",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("6bf5335f-c44e-4129-8e6f-0ad578f31e2d"),

                ConcurrencyStamp = "2612aab8-a133-4285-8d1f-2b352e680b69",

                Name = "Registered Users",

                NormalizedName = "REGISTERED USERS",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("f1420b89-34e2-4093-bcd5-0bc9c1f9ce94"),

                ConcurrencyStamp = "e50303b7-211d-4d31-91ad-9b645640ec9a",

                Name = "Test Role",

                NormalizedName = "TEST ROLE",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),

                ConcurrencyStamp = "73a5ff5a-0ea0-463a-af8f-a5a691cad43f",

                Name = "Administrators",

                NormalizedName = "ADMINISTRATORS",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("67eb6f56-623b-4d2a-b602-4bed0388995c"),

                ConcurrencyStamp = "fa6cd004-475c-46a6-a7fc-445ba74b5d29",

                Name = "Site Editor",

                NormalizedName = "SITE EDITOR",
            });

            _dbContext.Set<Role>
            ().Add(new Role
            {

                Id = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),

                ConcurrencyStamp = "20d0f2e5-8f06-4e0c-a233-a37d90e036bb",

                Name = "All Users",

                NormalizedName = "ALL USERS",
            });

            //SiteSetting

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("c8cab3fb-b9af-4a6b-b67e-0c0ebce46276"),

                SettingName = "TwitterConsumerSecret",

                SettingValue = "JyYFVSYG5HMNQNLynF1JKUQY0HueOS3DPsTxMkHQiGBuhumgbT",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("b9a59154-2cb1-480b-9504-2b9a8528e715"),

                SettingName = "EnableFacebookAuth",

                SettingValue = "true",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("bc5c0288-2798-4c64-92f0-3210af2cd993"),

                SettingName = "LoginPageId",

                SettingValue = "62328d72-ad82-4de2-9a98-c954e5b30b28",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("b39359e5-f92d-4db0-8e67-39c9edc32796"),

                SettingName = "SiteDescription",

                SettingValue = "CMS for developing applications.",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("fcc0377f-1860-4ffc-956c-3b8d13acd63b"),

                SettingName = "DefaultAdminTheme",

                SettingValue = "[G]Skins/Skyline/Admin.cshtml",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("d9187b0c-fdce-4a8f-9fbf-447a1b445207"),

                SettingName = "SMTPPassword",

                SettingValue = "SgSkyDev2016!",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("2ba57644-7a61-455b-986b-44deba9c3dec"),

                SettingName = "SMTPServerAndPort",

                SettingValue = "smtp.sendgrid.net:587",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("e90697bd-4932-4be0-9f55-4f9003aaa00e"),

                SettingName = "DefaultLayoutId",

                SettingValue = "d6471f27-716c-4f6f-a10b-4acef3fa4da3",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("b0b844c3-9ea6-4b39-81a3-50f886439bb8"),

                SettingName = "SMTPUsername",

                SettingValue = "azure_688fbdd4abe35655f5310f126dcc5fb8@azure.com",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("acf876f1-a97e-401f-982e-5e6464b1538c"),

                SettingName = "SiteLanguage",

                SettingValue = "en-US",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("628978e0-2105-4aee-a57e-6b3ff3fba764"),

                SettingName = "EnableTwitterAuth",

                SettingValue = "true",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("4adfa2cb-dfda-4395-b45f-6b899e5a860a"),

                SettingName = "RegistrationPageId",

                SettingValue = "51a79e31-9bb1-4fa7-4da6-08d3c2d166ce",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("3f621d1f-9b96-4ebe-8e20-70289cc06ec8"),

                SettingName = "TwitterConsumerKey",

                SettingValue = "Ssy9jZH9xLEcTErXsQsDhPBjt",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("3ff5cff7-3183-4f03-9d64-83778edb92dd"),

                SettingName = "SiteName",

                SettingValue = "DeviserWI",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("fd5b0a57-6876-47ea-b237-94951390061a"),

                SettingName = "ConnectionLimit",

                SettingValue = null,
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("2b38fc22-a935-42c2-bc03-a0c5bccf4118"),

                SettingName = "SiteAdminEmail",

                SettingValue = "noreply@deviser.com",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("08c40572-06c6-4c12-932d-a1ba4a36710b"),

                SettingName = "HomePageId",

                SettingValue = "d5d5a9fd-511b-4025-b495-8908fb70c762",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("7161c7f6-53e3-492e-8075-a38fecb4a1fa"),

                SettingName = "MaxIdleTime",

                SettingValue = null,
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("0a3bbfaf-e27e-48a6-b015-aa5d395a7a06"),

                SettingName = "SMTPAuthentication",

                SettingValue = "Basic",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("2806bd6b-6dec-4385-a568-af20a51d241c"),

                SettingName = "DefaultAdminLayoutId",

                SettingValue = "af8ecc7d-e300-41af-b55a-deeb097836d2",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("cc2ebe54-a3d7-4b21-aa15-b890df72d618"),

                SettingName = "GoogleClientId",

                SettingValue = "685539106368-j41e0r6ks778s602e48q8r6au63q630f.apps.googleusercontent.com",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("c65f2a74-08ce-457c-a826-bbc39cb012df"),

                SettingName = "EnableGoogleAuth",

                SettingValue = "true",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("ebd27836-16b2-4135-86ec-cfbd5234f704"),

                SettingName = "SiteRoot",

                SettingValue = "/",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("f585c969-117b-42cd-8ca0-d1aa65cb3d77"),

                SettingName = "SMTPEnableSSL",

                SettingValue = "false",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("39d6c99d-6e93-4c91-8f5a-d286f0e43d6a"),

                SettingName = "FacebookAppSecret",

                SettingValue = "fa6d92d23445873c2cd81e3909eb7e96",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("9062b696-4abe-484d-8e6d-d9bf3863cbbb"),

                SettingName = "RedirectAfterLogout",

                SettingValue = "62328d72-ad82-4de2-9a98-c954e5b30b28",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("2ef47dba-8872-41ff-88be-df8862e4a037"),

                SettingName = "RedirectAfterLogin",

                SettingValue = "d5d5a9fd-511b-4025-b495-8908fb70c762",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("a843912d-7238-49bd-b54e-ec295035bc7b"),

                SettingName = "RegistrationEnabled",

                SettingValue = "true",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("01858632-5f4c-4cda-9ac4-f0eae233f3ae"),

                SettingName = "GoogleClientSecret",

                SettingValue = "YB0nc-sxnD5RGc5MIq8KJ52f",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("b09c118d-d2d2-4825-bc6c-f51efe93b3a2"),

                SettingName = "DefaultTheme",

                SettingValue = "[G]Skins/Skyline/Default.cshtml",
            });

            _dbContext.Set<SiteSetting>
            ().Add(new SiteSetting
            {

                Id = Guid.Parse("0dde627e-e1e6-4bdf-85e7-fe794714649f"),

                SettingName = "FacebookAppId",

                SettingValue = "805579299652232",
            });

            //User

            _dbContext.Set<User>
            ().Add(new User
            {

                Id = Guid.Parse("715cf6d2-f730-45b0-aa7e-2a14b647f3ba"),

                AccessFailedCount = 0,

                ConcurrencyStamp = "26ea451a-83c1-408f-b3b4-7655256e5489",

                Email = "sky.karthick+2@gmail.com",

                EmailConfirmed = false,

                FirstName = "test",

                LastName = "user 1",

                LockoutEnabled = true,

                NormalizedEmail = "SKY.KARTHICK+2@GMAIL.COM",

                NormalizedUserName = "SKY.KARTHICK+2@GMAIL.COM",

                PasswordHash = "AQAAAAEAACcQAAAAEBud9KErt8miudTi/OvindaSC9T2oMtqVuzZYCfeYHVaZTAuFyGWdzelEsm9F2RBlQ==",

                PhoneNumber = null,

                PhoneNumberConfirmed = false,

                SecurityStamp = "236951f3-bc0c-47b5-91a6-4ec18824f73b",

                TwoFactorEnabled = false,

                UserName = "sky.karthick+2@gmail.com",
            });

            _dbContext.Set<User>
            ().Add(new User
            {

                Id = Guid.Parse("cc6e4b03-5b96-4582-baa5-68ae2592198d"),

                AccessFailedCount = 0,

                ConcurrencyStamp = "2b3df658-4d1c-4913-8f62-30ef71fce09b",

                Email = "sky.karthick+1@gmail.com",

                EmailConfirmed = true,

                FirstName = "test",

                LastName = "user",

                LockoutEnabled = true,

                NormalizedEmail = "SKY.KARTHICK+1@GMAIL.COM",

                NormalizedUserName = "SKY.KARTHICK+1@GMAIL.COM",

                PasswordHash = "AQAAAAEAACcQAAAAEKvRAIwCa5bebkXKZgowel1KLWNX5iVsDL9o9dkUAfbzYkdc4bGOv/5keu0r8Bb3pg==",

                PhoneNumber = null,

                PhoneNumberConfirmed = false,

                SecurityStamp = "f22aedf4-d9cd-482e-a96f-4d82a48d9bae",

                TwoFactorEnabled = false,

                UserName = "sky.karthick+1@gmail.com",
            });

            _dbContext.Set<User>
            ().Add(new User
            {

                Id = Guid.Parse("c6206baf-9ae9-42c2-bda9-97adcf6c8afd"),

                AccessFailedCount = 0,

                ConcurrencyStamp = "2456f0fe-38aa-4076-a796-357bc867f7e2",

                Email = "admin@deviser",

                EmailConfirmed = false,

                FirstName = "System",

                LastName = "Admin",

                LockoutEnabled = true,

                NormalizedEmail = "ADMIN@DEVISER",

                NormalizedUserName = "ADMIN@DEVISER",

                PasswordHash = "AQAAAAEAACcQAAAAEPCnDlYC/uTgR9UQQIMqOuooeS93X5wMR/a2lwXES5xPgNQ0+W68zExObdlv12pdqQ==",

                PhoneNumber = null,

                PhoneNumberConfirmed = false,

                SecurityStamp = "a4d1d213-2209-4695-8c7b-adbb04daab2b",

                TwoFactorEnabled = true,

                UserName = "admin@deviser",
            });

            _dbContext.Set<User>
            ().Add(new User
            {

                Id = Guid.Parse("21485ff9-6651-41c5-9129-e2dcc034ed9c"),

                AccessFailedCount = 0,

                ConcurrencyStamp = "694cf5bc-8cfd-48a4-9ad6-2777721dd009",

                Email = "sky.karthick+3@gmail.com",

                EmailConfirmed = false,

                FirstName = "new",

                LastName = "test user",

                LockoutEnabled = true,

                NormalizedEmail = "SKY.KARTHICK+3@GMAIL.COM",

                NormalizedUserName = "SKY.KARTHICK+3@GMAIL.COM",

                PasswordHash = "AQAAAAEAACcQAAAAEBsx916wzZ0usnBAuPMrIQlNVCrpZerKiFMgFV/SrKCPWTUizP+Uh5GCetz6+8weyQ==",

                PhoneNumber = null,

                PhoneNumberConfirmed = false,

                SecurityStamp = "f309f3e4-4c53-4cfa-954e-99476e7148da",

                TwoFactorEnabled = false,

                UserName = "sky.karthick+3@gmail.com",
            });

            
            //PageContentTranslation
            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("0a547422-9850-47f9-26ba-08d3c7a6e20b"),

                ContentData = "{\"videoUrl\":\"https://www.youtube.com/watch?v=EA-jdbno19E\",\"imageUrl\":\"/assets/images/UserManual.jpg?67.87946570053545\",\"imageAltText\":null,\"focusPoint\":null}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("2281e807-cf0c-67cb-796e-d41a83761206"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("ccfd52dd-b595-4815-1326-08d3c9001523"),

                ContentData = "{\"content\":\"<h1>An open-source, cross-platform, rapid application and WCM development framework </h1><p>Deviser is an open-source unique framework for developing modern cross-platform applications and Web Content Management system based on ASP.NET Core. This platform enables developers to create modern web application and empowers any user groups to create dynamic contents by just drag-and-drop on a dynamic layout in no time. Deviser Platform is completely free and Open Source on Github.</p>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("2eb17f68-bf88-0767-46e6-4d7e26b3cbaa"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("211241d1-a22a-4ee5-7699-08d57fbab186"),

                ContentData = "{\"items\":[{\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-feather\\\"><path d=\\\"M20.24 12.24a6 6 0 0 0-8.49-8.49L5 10.5V19h8.5z\\\"></path><line x1=\\\"16\\\" y1=\\\"8\\\" x2=\\\"2\\\" y2=\\\"22\\\"></line><line x1=\\\"17\\\" y1=\\\"15\\\" x2=\\\"9\\\" y2=\\\"15\\\"></line></svg>\",\"title\":\"Lightweight\",\"description\":\"Deviser is built on ASP.NET core with minimal dependencies, hence the framework is lightweight.\",\"id\":\"3b8f4491-f6d4-830e-2cfe-57e6614068b9\",\"viewOrder\":1},{\"title\":\"Dynamic Layout\",\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-layout\\\"><rect x=\\\"3\\\" y=\\\"3\\\" width=\\\"18\\\" height=\\\"18\\\" rx=\\\"2\\\" ry=\\\"2\\\"></rect><line x1=\\\"3\\\" y1=\\\"9\\\" x2=\\\"21\\\" y2=\\\"9\\\"></line><line x1=\\\"9\\\" y1=\\\"21\\\" x2=\\\"9\\\" y2=\\\"9\\\"></line></svg>\",\"description\":\"Create any number of layouts with dynamic layout elements (Placeholders) and get full control over HTML and CSS\",\"id\":\"bd5663f5-8fb1-8a0a-107f-b085713a486e\",\"viewOrder\":2},{\"title\":\"Do Not Repeat Yourself(DRY)\",\"description\":\"Do not worry about writing tons of code. Deviser provides a solid framework to reuse modules, contents and layouts\",\"id\":\"d90be6c1-8824-63ad-2fc7-2a2859000eba\",\"viewOrder\":3,\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-repeat\\\"><polyline points=\\\"17 1 21 5 17 9\\\"></polyline><path d=\\\"M3 11V9a4 4 0 0 1 4-4h14\\\"></path><polyline points=\\\"7 23 3 19 7 15\\\"></polyline><path d=\\\"M21 13v2a4 4 0 0 1-4 4H3\\\"></path></svg>\"},{\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-x\\\"><line x1=\\\"18\\\" y1=\\\"6\\\" x2=\\\"6\\\" y2=\\\"18\\\"></line><line x1=\\\"6\\\" y1=\\\"6\\\" x2=\\\"18\\\" y2=\\\"18\\\"></line></svg>\",\"title\":\"Cross Platform\",\"description\":\"Build web application and websites that run on Windows, Linux and macOS\",\"id\":\"6e5d82ed-eaf0-37e7-bb40-0fba7e3f9186\",\"viewOrder\":4},{\"description\":\"Create your own content types and empower non-technical users to manage complex contents at any extent.\",\"id\":\"6f4111fe-d3d4-2789-6ffe-2d9c628dea0b\",\"viewOrder\":5,\"title\":\"Dynamic Content\",\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-edit-3\\\"><polygon points=\\\"14 2 18 6 7 17 3 17 3 13 14 2\\\"></polygon><line x1=\\\"3\\\" y1=\\\"22\\\" x2=\\\"21\\\" y2=\\\"22\\\"></line></svg>\"},{\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-package\\\"><path d=\\\"M12.89 1.45l8 4A2 2 0 0 1 22 7.24v9.53a2 2 0 0 1-1.11 1.79l-8 4a2 2 0 0 1-1.79 0l-8-4a2 2 0 0 1-1.1-1.8V7.24a2 2 0 0 1 1.11-1.79l8-4a2 2 0 0 1 1.78 0z\\\"></path><polyline points=\\\"2.32 6.16 12 11 21.68 6.16\\\"></polyline><line x1=\\\"12\\\" y1=\\\"22.76\\\" x2=\\\"12\\\" y2=\\\"11\\\"></line><line x1=\\\"7\\\" y1=\\\"3.5\\\" x2=\\\"17\\\" y2=\\\"8.5\\\"></line></svg>\",\"description\":\"Develop independent modules based on ASP.NET MVC and place it on the Dynamic Layouts\",\"title\":\"Modular\",\"id\":\"577cb188-bcc4-7559-be36-ac3831e708bd\",\"viewOrder\":6},{\"title\":\"Multilingual\",\"id\":\"6bd307a0-a278-f2ae-471a-0251ca8a58a5\",\"viewOrder\":7,\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-flag\\\"><path d=\\\"M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z\\\"></path><line x1=\\\"4\\\" y1=\\\"22\\\" x2=\\\"4\\\" y2=\\\"15\\\"></line></svg>\",\"description\":\"The platform allows to manage pages and contents in any number of languages easily\"},{\"title\":\"Authentication and Authorization\",\"description\":\"Authentication and authorization are built-in within the platform.\",\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-users\\\"><path d=\\\"M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2\\\"></path><circle cx=\\\"9\\\" cy=\\\"7\\\" r=\\\"4\\\"></circle><path d=\\\"M23 21v-2a4 4 0 0 0-3-3.87\\\"></path><path d=\\\"M16 3.13a4 4 0 0 1 0 7.75\\\"></path></svg>\",\"id\":\"32284797-4520-3e57-29b5-cb859e3969d1\",\"viewOrder\":8},{\"icon\":\"<svg xmlns=\\\"http://www.w3.org/2000/svg\\\" width=\\\"24\\\" height=\\\"24\\\" viewBox=\\\"0 0 24 24\\\" fill=\\\"none\\\" stroke=\\\"currentColor\\\" stroke-width=\\\"2\\\" stroke-linecap=\\\"round\\\" stroke-linejoin=\\\"round\\\" class=\\\"feather feather-move\\\"><polyline points=\\\"5 9 2 12 5 15\\\"></polyline><polyline points=\\\"9 5 12 2 15 5\\\"></polyline><polyline points=\\\"15 19 12 22 9 19\\\"></polyline><polyline points=\\\"19 9 22 12 19 15\\\"></polyline><line x1=\\\"2\\\" y1=\\\"12\\\" x2=\\\"22\\\" y2=\\\"12\\\"></line><line x1=\\\"12\\\" y1=\\\"2\\\" x2=\\\"12\\\" y2=\\\"22\\\"></line></svg>\",\"title\":\"Drag-and-Drop\",\"description\":\"Built any kind of complex pages by just drag-and-drop content and modules on a dynamic layout.\",\"id\":\"3a6f1b06-9217-c58e-c35c-cc72f76fba99\",\"viewOrder\":9}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("3d215aa4-9c5b-a93e-1b7c-2df844467824"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("aca964dd-5239-4867-9dd0-08d5854030ba"),

                ContentData = "{\"items\":[],\"link\":{\"linkType\":\"URL\",\"url\":\"https://github.com/karthicksundararajan\",\"isNewWindow\":true,\"linkText\":\"Github\"},\"iconClass\":\"fa fa-github\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("c9916a98-17b6-f120-f38b-9a028cff7382"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("1dcfc2c9-bb2a-4fe6-7fde-08d586a9509c"),

                ContentData = "{\"items\":[],\"link\":{\"linkType\":\"URL\",\"linkText\":\"Get Started\",\"url\":\"/docs\"}}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("108d471d-e6ac-a321-c2b0-256a079e90df"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("7743ba96-3c46-47cf-48a9-08d5885ab545"),

                ContentData = "{\"items\":[{\"title\":\"Lightweight\",\"description\":\"Deviser is built on ASP.NET core with minimal dependencies, hence the framework is lightweight.\",\"imageUrl\":\"/assets/images/rawpixel-com-561415-unsplash.jpg?9.85880129268255\",\"imageAltText\":\"Lightweight\",\"id\":\"4312fbd1-88ab-26ff-8899-0051a2d0c668\",\"viewOrder\":1},{\"title\":\"Dynamic\",\"description\":\"Create any number of layouts with dynamic layout elements (Placeholders) and get full control over HTML and CSS\",\"imageUrl\":\"/assets/images/sai-kiran-anagani-61187-unsplash.jpg?72.3694811957373\",\"id\":\"dc82fdce-57b0-017b-f2b0-e12e81166efe\",\"viewOrder\":2},{\"title\":\"Cross Platform\",\"description\":\"Build web application and websites that run on Windows, Linux and macOS\",\"imageUrl\":\"/assets/images/spacex-71873-unsplash.jpg?44.49516821171806\",\"id\":\"085a2e19-df2a-1b48-fff0-3d3c31585b19\",\"viewOrder\":3}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("a032e37a-ac20-22e4-81af-72faed1cc13c"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("a5b5b79d-58cb-4fc5-48aa-08d5885ab545"),

                ContentData = "{\"items\":[{\"icon\":\"fa fa-puzzle-piece\",\"title\":\"Modular\",\"description\":\"Develop independent modules based on ASP.NET MVC and place it on the Dynamic Layouts\",\"id\":\"a956bc92-638c-b4fa-637b-bd12f584ca25\",\"viewOrder\":1},{\"title\":\"Multilingual\",\"description\":\"The platform allows to manage pages and contents in any number of languages easily\",\"icon\":\"fa fa-language\",\"id\":\"e2b70ef6-85b9-15fd-20ac-2fa3474b1a7c\",\"viewOrder\":2},{\"icon\":\"fa fa-arrows\",\"title\":\"Drag-and-Drop\",\"description\":\"Built any kind of complex pages by just drag-and-drop content and modules on a dynamic layout.\",\"id\":\"3bc938e3-883d-8f5d-d2b6-2acf1c3346af\",\"viewOrder\":3}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("deae8c01-cc70-ebc1-b165-af94417eae61"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("ab88e931-c51c-4996-26f0-08d590420a00"),

                ContentData = "{\"items\":[],\"content\":\"<h2>Features</h2>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("3ae46227-d6dc-b5da-9a31-87494a1ee576"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("9517fc8b-f221-4095-afc6-08d5932bbde0"),

                ContentData = "{\"items\":[],\"content\":\"<h1>An open-source, cross-platform, rapid application and WCM development framework </h1><p>Deviser is an open-source unique framework for developing modern cross-platform applications and Web Content Management system based on ASP.NET Core. This platform enables developers to create modern web application and empowers any user groups to create dynamic contents by just drag-and-drop on a dynamic layout in no time. Deviser Platform is completely free and Open Source on Github.</p>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("2fcd1f08-42b0-fe77-bf67-0fa01148eef3"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("1135b8a5-91e3-4b20-afc7-08d5932bbde0"),

                ContentData = "{\"items\":[],\"imageUrl\":\"/assets/images/UserManual.jpg?71.79668362564576\",\"videoUrl\":\"https://www.youtube.com/watch?v=EA-jdbno19E\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("42a6b892-6cc9-10ea-322e-edfc5f7f4c36"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("7652b301-87e4-48a2-74c0-08d59b27dc3d"),

                ContentData = "{\"items\":[{\"imageUrl\":\"/assets/images/alex-iby-558878-unsplash.jpg?95.33394332472156\",\"id\":\"72a3d536-603a-7340-31ff-53d6ae257402\",\"viewOrder\":1,\"description\":\"Lorem ipsum dolor sit amet, eum et debet moderatius, pro ne labitur elaboraret disputationi.\"},{\"imageUrl\":\"/assets/images/jason-blackeye-428759-unsplash.jpg?71.13357858472058\",\"description\":\"Odio impedit eu eam, clita admodum vituperatoribus ei eam, invidunt repudiandae consequuntur ex vis.\",\"id\":\"78b2ea9d-811a-064e-a526-a0c3ba0a306f\",\"viewOrder\":2},{\"imageUrl\":\"/assets/images/marcus-cramer-426500-unsplash.jpg?15.03560765820513\",\"description\":\"Congue sensibus dissentiet ei eum. An est fabulas phaedrum, an illud iudicabit consulatu ius, no per nominavi ocurreret.\",\"id\":\"c40a1aa8-1e7e-2b1e-bb8a-afe2f27aacfa\",\"viewOrder\":3},{\"imageUrl\":\"/assets/images/bella-huang-596578-unsplash.jpg?37.581091708642255\",\"description\":\"Vis illud prompta ceteros id, at ius accumsan reformidans. Sea ubique ridens deterruisset eu\",\"id\":\"86a4c05a-1e64-00ff-d797-f78e1e87df4d\",\"viewOrder\":4},{\"imageUrl\":\"/assets/images/guilherme-stecanella-368391-unsplash.jpg?83.26263688634276\",\"description\":\"Ea nam adhuc prompta impedit, his in mucius facilis, diam possim voluptaria ei has\",\"id\":\"efb4aea6-1d2b-1392-f659-c051a0ea49ca\",\"viewOrder\":5},{\"imageUrl\":\"/assets/images/richard-lee-500313-unsplash.jpg?11.945052612358609\",\"description\":\"Illud partem in sit, meliore adolescens in pro, ei eros mollis noluisse vix.\",\"id\":\"a7c61a1d-58d0-9673-ec81-1b15e5807c1a\",\"viewOrder\":6},{\"imageUrl\":\"/assets/images/axel-houmadi-325303-unsplash.jpg?98.03875512687345\",\"description\":\"Dicit accusam at vis. Duo dissentias dissentiunt ut, omnis inermis has ei, an dico intellegebat nam.\",\"id\":\"f482bada-80bc-0b9a-98c8-e75090b8bbf4\",\"viewOrder\":7},{\"imageUrl\":\"/assets/images/wladislaw-sokolowskij-584523-unsplash.jpg?10.510457008413155\",\"description\":\"Cu duo ubique disputationi, solum porro in mei, mea ea tamquam disputando.\",\"id\":\"f17146cb-1c7d-6044-cc10-418c64ae1381\",\"viewOrder\":8},{\"imageUrl\":\"/assets/images/rares-c-514851-unsplash.jpg?39.24327578868123\",\"description\":\"Commodo oportere ea nec, est ancillae dissentiet no. No sed paulo suscipit facilisis, vix ut elitr detraxit dignissim, at pri evertitur interesset.\",\"id\":\"c13006e4-cd8b-72db-4eb5-a246bd4f4739\",\"viewOrder\":9}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("49ff3342-d09e-8965-07b8-27289395cc7f"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("a47ad361-c122-4d15-e0be-08d59fe75845"),

                ContentData = "{\"items\":[],\"content\":\"<h2>Our Projects</h2><p>Inventore cillum soluta inceptos eos platea, soluta class laoreet repellendus imperdiet optio.</p>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("58f3b967-ecf6-3870-46e4-3c0e1d70fe21"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("60382382-2360-4226-ce4f-08d5a2c9ab96"),

                ContentData = "{\"items\":[{\"title\":\"Home\",\"description\":\"Et et consectetur ipsum labore excepteur est proident excepteur ad velit occaecat qui minim occaecat veniam. Fugiat veniam incididunt anim aliqua enim pariatur veniam sunt est aute sit dolor anim. Velit non irure adipisicing aliqua ullamco irure incididunt irure non esse consectetur nostrud minim non minim occaecat. Amet duis do nisi duis veniam non est eiusmod tempor incididunt tempor dolor ipsum in qui sit. Exercitation mollit sit culpa nisi culpa non adipisicing reprehenderit do dolore. Duis reprehenderit occaecat anim ullamco ad duis occaecat ex.\",\"id\":\"c44f3030-57e6-7f19-2706-1f212d26b7b6\",\"viewOrder\":1},{\"description\":\"Nulla est ullamco ut irure incididunt nulla Lorem Lorem minim irure officia enim reprehenderit. Magna duis labore cillum sint adipisicing exercitation ipsum. Nostrud ut anim non exercitation velit laboris fugiat cupidatat. Commodo esse dolore fugiat sint velit ullamco magna consequat voluptate minim amet aliquip ipsum aute laboris nisi. Labore labore veniam irure irure ipsum pariatur mollit magna in cupidatat dolore magna irure esse tempor ad mollit. Dolore commodo nulla minim amet ipsum officia consectetur amet ullamco voluptate nisi commodo ea sit eu.\",\"title\":\"Profile\",\"id\":\"98eedcec-80ab-a41f-ac4e-fab2762e46f2\",\"viewOrder\":2},{\"title\":\"Contact\",\"description\":\"Sint sit mollit irure quis est nostrud cillum consequat Lorem esse do quis dolor esse fugiat sunt do. Eu ex commodo veniam Lorem aliquip laborum occaecat qui Lorem esse mollit dolore anim cupidatat. Deserunt officia id Lorem nostrud aute id commodo elit eiusmod enim irure amet eiusmod qui reprehenderit nostrud tempor. Fugiat ipsum excepteur in aliqua non et quis aliquip ad irure in labore cillum elit enim. Consequat aliquip incididunt ipsum et minim laborum laborum laborum et cillum labore. Deserunt adipisicing cillum id nulla minim nostrud labore eiusmod et amet. Laboris consequat consequat commodo non ut non aliquip reprehenderit nulla anim occaecat. Sunt sit ullamco reprehenderit irure ea ullamco Lorem aute nostrud magna.\",\"id\":\"94c3e7c0-b4a6-15b4-1e84-784f8f4e70a3\",\"viewOrder\":3}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("5416d100-4f46-c549-914c-29e94f6ee06a"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("ba4acd04-d3d6-4f0b-ce50-08d5a2c9ab96"),

                ContentData = "{\"items\":[{\"description\":\"Lorem ipsum dolor sit amet, in modo nobis euismod cum, cu per malis tantas, in autem doming fuisset cum. At meis persecuti cum. Et eum viderer patrioque, consul aeterno veritus in has. Ex ius ignota rationibus liberavisse, mea ei nonumes mediocrem, mei te offendit adipiscing. Lobortis concludaturque in quo, ad sed diam disputationi. Ubique graeco per ut, epicuri antiopam eu cum. Eros propriae id est.\",\"title\":\"Lorem ipsum\",\"id\":\"801c6906-fd1c-8c2b-541c-c1544a486d04\",\"viewOrder\":1},{\"description\":\"Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.\",\"title\":\"Anim pariatur\",\"id\":\"d4ccc44f-e6f8-0f23-e6a5-05bbbc68d98d\",\"viewOrder\":2},{\"description\":\"His clita quando veritus ei, qui graece deterruisset ad. Tollit eleifend ocurreret eos ut. No dicat summo eam, usu ad oratio facilisi imperdiet. Dicat civibus te pro, etiam audire ei vel, ex sumo malorum feugait ius.\",\"title\":\"His clita\",\"id\":\"951d399c-6efd-94cb-4584-e1505f6d4601\",\"viewOrder\":3}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("033ab6c7-a8e1-536e-9794-27d0752031c6"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("58abc8bf-9dcd-4f8c-ce51-08d5a2c9ab96"),

                ContentData = "{\"items\":[{\"latitude\":\"47.377923\",\"longitude\":\"8.5401898\",\"title\":\"Zurich Head Office\",\"locationName\":\"Zurich Mainstation\",\"address\":\"Bahnhofstrasse 1\",\"postalCodeAndCity\":\"8001 Zürich\",\"id\":\"ddfa77d7-d430-9b55-cdb7-d104bc034646\",\"viewOrder\":1}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("a0335b80-b2cc-a282-ea78-01924eee2513"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("6a37daa2-5a07-4a83-04f4-08d5a39db8f3"),

                ContentData = "{\"items\":[{\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"},\"title\":\"UI/UX Design\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"pencil\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 512 512\\\" class=\\\"svg-inline--fa fa-pencil fa-w-16\\\"><path fill=\\\"#495057\\\" d=\\\"M493.255 56.236l-37.49-37.49c-24.993-24.993-65.515-24.994-90.51 0L12.838 371.162.151 485.346c-1.698 15.286 11.22 28.203 26.504 26.504l114.184-12.687 352.417-352.417c24.992-24.994 24.992-65.517-.001-90.51zm-95.196 140.45L174 420.745V386h-48v-48H91.255l224.059-224.059 82.745 82.745zM126.147 468.598l-58.995 6.555-30.305-30.305 6.555-58.995L63.255 366H98v48h48v34.745l-19.853 19.853zm344.48-344.48l-49.941 49.941-82.745-82.745 49.941-49.941c12.505-12.505 32.748-12.507 45.255 0l37.49 37.49c12.506 12.506 12.507 32.747 0 45.255z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"id\":\"c3204050-38c6-17a9-4253-5e1bde665978\",\"viewOrder\":1},{\"title\":\"Premium Support\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"life-ring\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 512 512\\\" class=\\\"svg-inline--fa fa-life-ring fa-w-16\\\"><path fill=\\\"#495057\\\" d=\\\"M256 8C119.033 8 8 119.033 8 256s111.033 248 248 248 248-111.033 248-248S392.967 8 256 8zm168.766 113.176l-62.885 62.885a128.711 128.711 0 0 0-33.941-33.941l62.885-62.885a217.323 217.323 0 0 1 33.941 33.941zM256 352c-52.935 0-96-43.065-96-96s43.065-96 96-96 96 43.065 96 96-43.065 96-96 96zM363.952 68.853l-66.14 66.14c-26.99-9.325-56.618-9.33-83.624 0l-66.139-66.14c66.716-38.524 149.23-38.499 215.903 0zM121.176 87.234l62.885 62.885a128.711 128.711 0 0 0-33.941 33.941l-62.885-62.885a217.323 217.323 0 0 1 33.941-33.941zm-52.323 60.814l66.139 66.14c-9.325 26.99-9.33 56.618 0 83.624l-66.139 66.14c-38.523-66.715-38.5-149.229 0-215.904zm18.381 242.776l62.885-62.885a128.711 128.711 0 0 0 33.941 33.941l-62.885 62.885a217.366 217.366 0 0 1-33.941-33.941zm60.814 52.323l66.139-66.14c26.99 9.325 56.618 9.33 83.624 0l66.14 66.14c-66.716 38.524-149.23 38.499-215.903 0zm242.776-18.381l-62.885-62.885a128.711 128.711 0 0 0 33.941-33.941l62.885 62.885a217.323 217.323 0 0 1-33.941 33.941zm52.323-60.814l-66.14-66.14c9.325-26.99 9.33-56.618 0-83.624l66.14-66.14c38.523 66.715 38.5 149.229 0 215.904z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"id\":\"2c7122c9-ecab-153e-7c71-aac8f6d3b3b8\",\"viewOrder\":2,\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"}},{\"title\":\"Graphic Design\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"video\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 576 512\\\" class=\\\"svg-inline--fa fa-video fa-w-18\\\"><path fill=\\\"#495057\\\" d=\\\"M543.9 96c-6.2 0-12.5 1.8-18.2 5.7L416 171.6v-59.8c0-26.4-23.2-47.8-51.8-47.8H51.8C23.2 64 0 85.4 0 111.8v288.4C0 426.6 23.2 448 51.8 448h312.4c28.6 0 51.8-21.4 51.8-47.8v-59.8l109.6 69.9c5.7 4 12.1 5.7 18.2 5.7 16.6 0 32.1-13 32.1-31.5v-257c.1-18.5-15.4-31.5-32-31.5zM384 400.2c0 8.6-9.1 15.8-19.8 15.8H51.8c-10.7 0-19.8-7.2-19.8-15.8V111.8c0-8.6 9.1-15.8 19.8-15.8h312.4c10.7 0 19.8 7.2 19.8 15.8v288.4zm160-15.7l-1.2-1.3L416 302.4v-92.9L544 128v256.5z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"},\"id\":\"587735bf-9c36-9b47-c8ff-6aa1e1340789\",\"viewOrder\":3},{\"title\":\"Secure Applications\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"key\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 512 512\\\" class=\\\"svg-inline--fa fa-key fa-w-16\\\"><path fill=\\\"#495057\\\" d=\\\"M336 32c79.529 0 144 64.471 144 144s-64.471 144-144 144c-18.968 0-37.076-3.675-53.661-10.339L240 352h-48v64h-64v64H32v-80l170.339-170.339C195.675 213.076 192 194.968 192 176c0-79.529 64.471-144 144-144m0-32c-97.184 0-176 78.769-176 176 0 15.307 1.945 30.352 5.798 44.947L7.029 379.716A24.003 24.003 0 0 0 0 396.686V488c0 13.255 10.745 24 24 24h112c13.255 0 24-10.745 24-24v-40h40c13.255 0 24-10.745 24-24v-40h19.314c6.365 0 12.47-2.529 16.971-7.029l30.769-30.769C305.648 350.055 320.693 352 336 352c97.184 0 176-78.769 176-176C512 78.816 433.231 0 336 0zm48 108c11.028 0 20 8.972 20 20s-8.972 20-20 20-20-8.972-20-20 8.972-20 20-20m0-28c-26.51 0-48 21.49-48 48s21.49 48 48 48 48-21.49 48-48-21.49-48-48-48z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"},\"id\":\"78ae3721-8b9e-aeac-e741-83fe1bac5471\",\"viewOrder\":4},{\"title\":\"Creative Solution\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"lightbulb\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 384 512\\\" class=\\\"svg-inline--fa fa-lightbulb fa-w-12\\\"><path fill=\\\"#495057\\\" d=\\\"M192 80c0 8.837-7.164 16-16 16-35.29 0-64 28.71-64 64 0 8.837-7.164 16-16 16s-16-7.163-16-16c0-52.935 43.065-96 96-96 8.836 0 16 7.163 16 16zm176 96c0 101.731-51.697 91.541-90.516 192.674a23.722 23.722 0 0 1-5.484 8.369V464h-.018a23.99 23.99 0 0 1-5.241 14.574l-19.535 24.419A24 24 0 0 1 228.465 512h-72.93a24 24 0 0 1-18.741-9.007l-19.535-24.419A23.983 23.983 0 0 1 112.018 464H112v-86.997a24.153 24.153 0 0 1-5.54-8.478c-38.977-101.401-90.897-90.757-90.457-193.822C16.415 78.01 95.306 0 192 0c97.195 0 176 78.803 176 176zM240 448h-96v12.775L159.38 480h65.24L240 460.775V448zm0-64h-96v32h96v-32zm96-208c0-79.59-64.424-144-144-144-79.59 0-144 64.423-144 144 0 87.475 44.144 70.908 86.347 176h115.306C291.779 247.101 336 263.222 336 176z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"},\"id\":\"a7fbf9d0-3e3f-d32f-8d75-e56eb4dd2b8b\",\"viewOrder\":5},{\"title\":\"Quality Assurance\",\"icon\":\"<svg aria-hidden=\\\"true\\\" data-prefix=\\\"fal\\\" data-icon=\\\"check\\\" role=\\\"img\\\" xmlns=\\\"http://www.w3.org/2000/svg\\\" viewBox=\\\"0 0 448 512\\\" class=\\\"svg-inline--fa fa-check fa-w-14\\\"><path fill=\\\"#495057\\\" d=\\\"M413.505 91.951L133.49 371.966l-98.995-98.995c-4.686-4.686-12.284-4.686-16.971 0L6.211 284.284c-4.686 4.686-4.686 12.284 0 16.971l118.794 118.794c4.686 4.686 12.284 4.686 16.971 0l299.813-299.813c4.686-4.686 4.686-12.284 0-16.971l-11.314-11.314c-4.686-4.686-12.284-4.686-16.97 0z\\\" class=\\\"\\\" stroke=\\\"none\\\" stroke-width=\\\"1px\\\"></path></svg>\",\"description\":\"Nonummy augue culpa aenean inceptos sapiente justo alias quod nonummy veritatis impedit! Sit minim nibh mollis.\",\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"8dd34791-fad4-4f9d-a42c-08d587845981\"},\"id\":\"2d95bad7-f481-df70-7287-68e76bae82f1\",\"viewOrder\":6}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("385b16f5-4350-ff0d-baa6-5e0ad96ea608"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("c2c3f60a-15f1-400e-a7ae-08d5a5688a3b"),

                ContentData = "{\"items\":[{\"imageUrl\":\"/assets/images/img_avatar.png?78.32406559902098\",\"name\":\"Peter Muster\",\"position\":\"CEO\",\"id\":\"ff434365-c012-92ed-9586-4b138ea0ff0d\",\"viewOrder\":1},{\"imageUrl\":\"/assets/images/img_avatar2.png?33.96990558204536\",\"name\":\"Petra Muster\",\"position\":\"CFO\",\"id\":\"92a5fd89-6058-401e-56c8-c8bcca14b2d3\",\"viewOrder\":2},{\"imageUrl\":\"/assets/images/img_avatar.png?88.67899420627126\",\"imageAltText\":\"John Muster\",\"name\":\"John Muster\",\"position\":\"CIO\",\"id\":\"5a0a0362-536b-23f1-6a10-6ebe37052ae5\",\"viewOrder\":3},{\"imageUrl\":\"/assets/images/img_avatar2.png?74.15355174404623\",\"name\":\"Christina Muster\",\"position\":\"CTO\",\"id\":\"c03a6009-44fc-4b6d-445c-1dc24f804e8b\",\"viewOrder\":4},{\"imageUrl\":\"/assets/images/img_avatar2.png?55.43743100362612\",\"name\":\"Petra Muster\",\"position\":\"Product Manager\",\"id\":\"96aa4477-55fe-067f-fc68-e6801d24905d\",\"viewOrder\":5},{\"imageUrl\":\"/assets/images/img_avatar.png?97.81804704426233\",\"name\":\"James Bond\",\"position\":\"Technical Lead\",\"id\":\"b164e845-b28f-7420-c53a-9ce258e93b91\",\"viewOrder\":6}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("9d66eb3d-8795-c015-4a29-e4951e84f5a8"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("7a70f31e-dead-4346-a7af-08d5a5688a3b"),

                ContentData = "{\"items\":[],\"content\":\"<h1>Our Team</h1><p>Lorem ipsum dolor sit amet, impetus appellantur pri cu. Pri te assum verterem maluisset.</p>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("b04a9f79-17a8-b1ff-e5f6-32b81e3936fd"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("c5fbcad7-e300-43f6-a7b0-08d5a5688a3b"),

                ContentData = "{\"items\":[],\"content\":\"<h1>Our Services</h1><p>Lorem ipsum dolor sit amet, impetus appellantur pri cu. Pri te assum verterem maluisset.</p>\"}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("ee52a5c6-5117-5312-59cf-e43d398fd95e"),
            });

            _dbContext.Set<PageContentTranslation>
            ().Add(new PageContentTranslation
            {

                Id = Guid.Parse("56a9e3d3-9868-4d2e-3feb-08d5b45c8591"),

                ContentData = "{\"items\":[{\"date\":\"2018-05-20T00:00:00+02:00\",\"shortDescription\":\"Today I am very happy to announce the official release of Deviser 1.0 alpha version. The office guide is available at <a href=\\\"#\\\">deviser.io/guide</a>\",\"title\":\"Deviser 1.0 Alpha is Out!\",\"id\":\"cee4d6b9-e67f-344c-2668-67cd6f0b0b46\",\"viewOrder\":1,\"link\":{\"linkType\":\"PAGE\",\"pageId\":\"c015c363-3973-4619-a437-08d587845981\",\"linkText\":\"Read more\"},\"imageUrl\":\"/assets/images/alex-iby-558878-unsplash.jpg?4.611297946884441\"}]}",

                CultureCode = "en-US",

                IsDeleted = false,

                PageContentId = Guid.Parse("64bf349e-df8e-f341-22d4-2e03fbaafa2a"),
            });

            //PageTranslation

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("c597d915-38e0-4c32-0615-08d3a367fbcc"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "User Management",

                Title = "User Management",

                URL = "Admin/UserManagement",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("56b72d88-5922-4635-0616-08d3a367fbcc"),

                Locale = "en-US",

                Description = "Security Roles",

                Keywords = null,

                Name = "Security Roles",

                Title = "Security Roles",

                URL = "Admin/SecurityRoles",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("8efd99d2-5004-44c6-0617-08d3a367fbcc"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Languages",

                Title = "Languages",

                URL = "Admin/Languages",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("56ff05c4-57f6-429c-c4ad-08d3a6adbc78"),

                Locale = "en-US",

                Description = "Content Types",

                Keywords = null,

                Name = "Content Types",

                Title = "Content Types",

                URL = "Admin/DynamicContent/ContentTypes",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("20d1b105-5c6d-4961-c4ae-08d3a6adbc78"),

                Locale = "en-US",

                Description = "Layout Types",

                Keywords = null,

                Name = "Layout Types",

                Title = "Layout Types",

                URL = "Admin/DynamicContent/LayoutTypes",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("9333f6a1-ba81-4b18-a922-08d3adc0bb30"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Option List",

                Title = "Option List",

                URL = "Admin/DynamicContent/OptionList",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("7dff05ab-1376-4ae6-09f0-08d3ae5877da"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Properties",

                Title = "Properties",

                URL = "Admin/DynamicContent/Properties",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("bb858c11-6779-406d-e941-08d3b4c8ff40"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Site Settings",

                Title = "Site Settings",

                URL = "Admin/SiteSettings",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("57942b7c-42a8-405e-aa52-08d3b8ab87fd"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Dynamic Content",

                Title = "Dynamic Content",

                URL = "Admin/DynamicContent",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("1322cf31-fae5-40de-d7b7-08d3bfd5ca3d"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Module Management",

                Title = "Module Management",

                URL = "Admin/ModuleManagement",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Register",

                Title = "Register",

                URL = "register",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("2624f356-ff17-49b4-9d18-08d52ace7d21"),

                Locale = "en-US",

                Description = "Recycle Bin",

                Keywords = null,

                Name = "Recycle Bin",

                Title = "Recycle Bin",

                URL = "Admin/RecycleBin",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("3b11b771-88ce-4755-c1b8-08d57d5f48f2"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Team",

                Title = "Team",

                URL = "Team",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Blog",

                Title = "Blog",

                URL = "Blog",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("4c099d48-8810-42e2-53d4-08d5869fb686"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Documentation",

                Title = "Documentation",

                URL = "Documentation",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("0c1a1106-6d75-4349-53d5-08d5869fb686"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Demo",

                Title = "Demo",

                URL = "Demo",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Home",

                Title = "Home",

                URL = "Demo/Home",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Projects",

                Title = "Projects",

                URL = "Demo/Projects",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Services",

                Title = "Services",

                URL = "Demo/Services",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Team",

                Title = "Team",

                URL = "Demo/Team",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("aaa7c6b9-03e8-424f-a42e-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Elements",

                Title = "Elements",

                URL = "Demo/Elements",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Accordion",

                Title = "Accordion",

                URL = "Demo/Elements/Accordion",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("11d8379e-20bc-4824-a430-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Slider",

                Title = "Slider",

                URL = "Demo/Elements/Slider",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("d5931c29-0761-4791-a431-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Map",

                Title = "Map",

                URL = "Demo/Elements/Map",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("4e5b71d2-c764-4ad8-a432-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Gallery",

                Title = "Gallery",

                URL = "Demo/Elements/Gallery",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("5a1b5cf5-adca-4de8-a433-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Richtext",

                Title = "Richtext",

                URL = "Demo/Elements/Richtext",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Testimonials",

                Title = "Testimonials",

                URL = "Demo/Elements/Testimonials",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Contact",

                Title = "Contact",

                URL = "Demo/Contact",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("bcd8f34d-0a61-4291-a436-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Features",

                Title = "Features",

                URL = "Demo/Elements/Features",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("c015c363-3973-4619-a437-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Introduction",

                Title = "Introduction",

                URL = "Documentation/Introduction",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("81b0f35b-6c3b-4068-a438-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Get Started",

                Title = "Get Started",

                URL = "Documentation/GetStarted",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("f5ecb954-c5cc-414b-a439-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "For Developers",

                Title = "For Developers",

                URL = "Documentation/ForDevelopers",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("fc6670ef-ab01-4bd2-a43a-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Administration",

                Title = "Administration",

                URL = "Documentation/Administration",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("d82459b7-58ad-4aef-a43b-08d587845981"),

                Locale = "en-US",

                Description = "How to create a page, publish a page, choose theme, delete page, restore page",

                Keywords = null,

                Name = "Page Management",

                Title = "Page Management",

                URL = "Documentation/Administration/PageManagement",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("44945c5e-9e39-4cc6-a43c-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "User Management",

                Title = "User Management",

                URL = "Documentation/Administration/UserManagement",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("842dd9b2-3cbb-4049-a43d-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Languages",

                Title = "Languages",

                URL = "Documentation/Administration/Languages",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("44e735ac-3321-404b-a43e-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Recycle Bin",

                Title = "Recycle Bin",

                URL = "Documentation/Administration/RecycleBin",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("539b960c-be4f-46eb-a43f-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Module Management",

                Title = "Module Management",

                URL = "Documentation/Administration/ModuleManagement",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("577d9ed0-a809-4696-a440-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Site Settings",

                Title = "Site Settings",

                URL = "Documentation/Administration/SiteSettings",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("b7e8dc81-333e-4d35-a441-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Dynamic Content",

                Title = "Dynamic Content",

                URL = "Documentation/Administration/DynamicContent",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("d82d4a2b-9899-4bdc-a442-08d587845981"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Dynamic Layout",

                Title = "Dynamic Layout",

                URL = "Documentation/Administration/DynamicLayout",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("847c808f-a597-4420-a443-08d587845981"),

                Locale = "en-US",

                Description = "View Edit and Layout Mode",

                Keywords = null,

                Name = "Concepts",

                Title = "Concepts",

                URL = "Documentation/Concepts",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("13b9fc1c-bd49-4123-a444-08d587845981"),

                Locale = "en-US",

                Description = "How to create custom module",

                Keywords = null,

                Name = "Custom Module",

                Title = "Custom Module",

                URL = "Documentation/CustomModule",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Tabs",

                Title = "Tabs",

                URL = "demo/elements/tabs",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Locale = "de-CH",

                Description = "Test description",

                Keywords = "Test keyword",

                Name = "",

                Title = "Starterseit",

                URL = "",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Locale = "en-US",

                Description = "Test description",

                Keywords = "Test keyword",

                Name = "Home",

                Title = "Deviser | Home",

                URL = "Home",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("dd650840-0ee7-46cf-abb5-8a1591dc0936"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Admin",

                Title = "Admin",

                URL = "Admin",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Login",

                Title = "Login",

                URL = "Login",
            });

            _dbContext.Set<PageTranslation>
            ().Add(new PageTranslation
            {

                PageId = Guid.Parse("c6dd6902-4a9c-4a38-8a05-febe76694993"),

                Locale = "en-US",

                Description = null,

                Keywords = null,

                Name = "Page Management",

                Title = "Page Management",

                URL = "Admin/PageManagement",
            });

            //Property

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("f4fff310-340a-437a-8ce3-08d54de42fea"),

                Label = "Column Width (M)",

                Name = "column_width",

                DefaultValue = "f2eafdde-4f79-a195-c1c1-0794e293fa27",

                Description = "Column width for extra small (XS) devices: ≥768px",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("fa1b0856-cea0-4245-e3f1-08d582dfee42"),

                Label = "Video Preview",

                Name = "video_preview",

                DefaultValue = null,

                Description = null,
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("0d9134a4-4eb6-40a8-0959-08d58bf2fdc6"),

                Label = "Direction",

                Name = "swiper_direction",

                DefaultValue = "954d54e1-5c95-da20-422e-9c31691631b2",

                Description = "Could be 'horizontal' or 'vertical' (for vertical slider).",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("278c0cc1-5212-4bfa-095a-08d58bf2fdc6"),

                Label = "Speed",

                Name = "swiper_speed",

                DefaultValue = "300",

                Description = "Duration of transition between slides (in ms)",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("89e09c02-9c55-4721-5531-08d58c515929"),

                Label = "Auto Height",

                Name = "swiper_autoHeight",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Set to true and slider wrapper will adopt its height to the height of the currently active slide",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("238b60e4-bcea-4171-5532-08d58c515929"),

                Label = "Round Lengths",

                Name = "swiper_roundLengths",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Set to true to round values of slides width and height to prevent blurry texts on usual resolution screens (if you have such)",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("c436196b-f19e-4e3b-5535-08d58c515929"),

                Label = "Effect",

                Name = "swiper_effect",

                DefaultValue = "196f5ee0-c955-7d7a-618d-27863e379a19",

                Description = "Tranisition effect. Could be \"slide\", \"fade\", \"cube\", \"coverflow\" or \"flip\"",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("4a81a097-a694-4db2-5538-08d58c515929"),

                Label = "Navigation",

                Name = "swiper_navigation",

                DefaultValue = "20e770b7-b245-16e6-925c-1ce3a036d1ae",

                Description = "Enable/Disable navigation",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("338675b6-6ffa-4f2d-5539-08d58c515929"),

                Label = "Pagination",

                Name = "swiper_pagination",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Enable/Disable pagination",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("7b2d6527-199b-406d-553b-08d58c515929"),

                Label = "Pagination Type",

                Name = "swiper_pagination_type",

                DefaultValue = "0a50497a-f3d8-53de-932d-2b30a390b125",

                Description = "String with type of pagination. Can be \"bullets\", \"fraction\", \"progressbar\" or \"custom\"",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("2157ff87-35a5-405c-553c-08d58c515929"),

                Label = "Scrollbar",

                Name = "swiper_scrollbar",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Enable/Disable scrollbar",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("4f6bf74f-b4eb-4b66-553d-08d58c515929"),

                Label = "Space Between",

                Name = "swiper_spaceBetween",

                DefaultValue = "0",

                Description = "Distance between slides in px.",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("84ac0204-f84e-4e1c-553e-08d58c515929"),

                Label = "Sliders per view",

                Name = "swiper_slidesPerView",

                DefaultValue = "1",

                Description = "Number of slides per view (slides visible at the same time on slider's container).",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("55f585c6-708a-4487-553f-08d58c515929"),

                Label = "Loop",

                Name = "swiper_loop",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Set to true to enable continuous loop mode",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("2c843984-7009-4287-5540-08d58c515929"),

                Label = "keyboard",

                Name = "swiper_keyboard",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Enable/Disable keyboard control",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("017c4f30-da51-4d18-5541-08d58c515929"),

                Label = "Mouse Wheel",

                Name = "swiper_mousewheel",

                DefaultValue = "13916b6e-9c6e-accb-c457-9d71c32909c0",

                Description = "Enable/Disable mousewheel",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05"),

                Label = "Column Width (XS)",

                Name = "column_width_xs",

                DefaultValue = "6597d3bd-0971-9d73-968b-64ff6e2eabda",

                Description = "Column width for extra small (XS) devices: <576px",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("3379dca0-ada6-4245-aeab-08d58c5c2d05"),

                Label = "Column Width (S)",

                Name = "column_width_s",

                DefaultValue = "e7cf0d7d-e66b-09cb-f338-720588431bee",

                Description = "Colum width for Small (S) devices: ≥576px",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("14539266-bd68-440f-aeac-08d58c5c2d05"),

                Label = "Column Width (L)",

                Name = "column_width_l",

                DefaultValue = "5913c029-6cd8-352b-eae0-6f3f2e146b01",

                Description = "Column width for Large (L) devices: ≥768px",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("f0ad85a3-ff8c-40c6-aead-08d58c5c2d05"),

                Label = "Column Width (XL)",

                Name = "column_width_xl",

                DefaultValue = "4e3ff987-b14b-0713-4243-053fc5787389",

                Description = "Column width for Extra Large (XL) devices: ≥1200px",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("789115a9-2b18-474d-0721-08d58e3bfd70"),

                Label = "From",

                Name = "from",

                DefaultValue = "",

                Description = "From Address for the Email",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("1acf1470-d421-4b91-0722-08d58e3bfd70"),

                Label = "Admin Email",

                Name = "cf_admin_email",

                DefaultValue = null,

                Description = "Contact Form Admin Email Address",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("16485fed-208b-456a-fd02-08d590dbe247"),

                Label = "Subject",

                Name = "subject",

                DefaultValue = null,

                Description = "Subject for email.",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("f645a11f-bc8c-4003-0e83-08d5932d4c52"),

                Label = "Image Width",

                Name = "image_width",

                DefaultValue = "300",

                Description = "Image Width",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("cb4b6f18-7267-41c3-0e84-08d5932d4c52"),

                Label = "Image Height",

                Name = "image_height",

                DefaultValue = "200",

                Description = "Image Height",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("217fbbe9-bf8e-4c49-c720-08d5a94c6f03"),

                Label = "View Template",

                Name = "cf_view_template",

                DefaultValue = "8f9ccd68-101d-cc14-ee4a-2676aaedc3f5",

                Description = "Contact Form View Templates",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f"),

                Label = "Admin Email Template",

                Name = "cf_admin_email_template",

                DefaultValue = "384df06f-63cd-effb-faf8-b152885ba305",

                Description = "Contact Form Email Template",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("aa417aea-c1f2-467c-338d-08d5a95294ba"),

                Label = "Contact Email Template",

                Name = "cf_contact_email_template",

                DefaultValue = "384df06f-63cd-effb-faf8-b152885ba305",

                Description = "CF Contact Email Template",
            });

            _dbContext.Set<Property>
            ().Add(new Property
            {

                Id = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),

                Label = "Css Class",

                Name = "css_class",

                DefaultValue = null,

                Description = null,
            });

            //RoleClaim

            //UserClaim

            //UserLogin

            //ContentTypeProperty

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                PropertyId = Guid.Parse("fa1b0856-cea0-4245-e3f1-08d582dfee42"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("0d9134a4-4eb6-40a8-0959-08d58bf2fdc6"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("278c0cc1-5212-4bfa-095a-08d58bf2fdc6"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("89e09c02-9c55-4721-5531-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("238b60e4-bcea-4171-5532-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("c436196b-f19e-4e3b-5535-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("4a81a097-a694-4db2-5538-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("338675b6-6ffa-4f2d-5539-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("7b2d6527-199b-406d-553b-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("2157ff87-35a5-405c-553c-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("4f6bf74f-b4eb-4b66-553d-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("84ac0204-f84e-4e1c-553e-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("55f585c6-708a-4487-553f-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("2c843984-7009-4287-5540-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("017c4f30-da51-4d18-5541-08d58c515929"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                PropertyId = Guid.Parse("f645a11f-bc8c-4003-0e83-08d5932d4c52"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                PropertyId = Guid.Parse("cb4b6f18-7267-41c3-0e84-08d5932d4c52"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("817fea8f-59e2-4b77-8e63-1ea002772893"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("f99a54f8-5704-4bc1-b287-3a930c9ece53"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("c49840f4-5a00-4d1d-86b7-7881e3841314"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("978bd890-7dbd-4ee0-9d86-8356dfadf4e6"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("69933d62-31ed-481e-be1f-95dfb8210027"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("f2e91a21-0864-4b16-b3de-9be08888b91f"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("a3e319ea-80b9-4800-9032-bb7ea09ed331"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("8d878db7-c3e2-4c39-b359-bd0e39d87df9"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("00332002-f2c7-401c-b59c-d0181eaf657b"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("b2c35761-a953-4bf7-bfb2-d0ea9e63786d"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<ContentTypeProperty>
            ().Add(new ContentTypeProperty
            {

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            //LayoutTypeProperty

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("f4fff310-340a-437a-8ce3-08d54de42fea"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("3de7dc0a-b6c1-4136-aeaa-08d58c5c2d05"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("3379dca0-ada6-4245-aeab-08d58c5c2d05"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("14539266-bd68-440f-aeac-08d58c5c2d05"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("f0ad85a3-ff8c-40c6-aead-08d58c5c2d05"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("a72ce8d2-5f5f-4176-0141-08d57ef792d3"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("5a0a5884-da84-4922-a02f-5828b55d5c92"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("4c98f160-d676-40a2-9b88-79fd1343f333"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("9341f92e-83d8-4afe-ad4a-a95deeda9ae3"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            _dbContext.Set<LayoutTypeProperty>
            ().Add(new LayoutTypeProperty
            {

                LayoutTypeId = Guid.Parse("43734210-943e-4f33-a161-f12260b8c001"),

                PropertyId = Guid.Parse("f5031c31-778b-45dd-bd33-eeb2a088d2bc"),
            });

            //ModuleAction

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("3bc79404-700a-47e1-ca1f-08d52ace68d7"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.RecycleBin.Controllers",

                DisplayName = "Recycle Bin",

                IconClass = "fa fa-recycle",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("d670ac96-2ab6-4036-4664-08d52acdf1a1"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ActionName = "Index",

                ControllerName = "ContactForm",

                ControllerNamespace = "Deviser.Modules.ContactForm.Controllers",

                DisplayName = "Contact",

                IconClass = "fa fa-user-circle",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("ae7afca8-56f6-4381-822c-1a04022c779b"),

                ActionName = "Modules",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.ModuleManagement.Controllers",

                DisplayName = "Modules",

                IconClass = "fa fa-puzzle-piece",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f271f063-aa57-4ee0-95a4-d1417fab15c4"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("1b56fde5-4494-4cf7-88db-2dc7b942284b"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.FileManager.Controllers",

                DisplayName = "File Manager",

                IconClass = "fa fa-files-o",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f07dbddf-4937-42b8-9bee-9c0713128013"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("57415ac9-9141-495a-a25d-4a80f1c5827a"),

                ActionName = "LayoutTypes",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.ContentManagement.Controllers",

                DisplayName = "Layout Types",

                IconClass = "fa fa-columns",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("7f2e81f9-90c3-4247-a545-658cc370caf5"),

                ActionName = "Register",

                ControllerName = "Account",

                ControllerNamespace = "Deviser.Modules.Security.Controllers",

                DisplayName = "Register",

                IconClass = "fa fa-pencil-square-o",

                IconImage = null,

                IsDefault = false,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("724a7aa2-4916-40dc-9579-7afc31589d12"),

                ActionName = "SiteSettings",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.SiteManagement.Controllers",

                DisplayName = "Site Settings",

                IconClass = "fa fa-cog",

                IconImage = null,

                IsDefault = false,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("e99086da-297e-4fdd-a84c-74c663baf9ae"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("54df0a1f-99b0-4847-91f5-7cd187818fe3"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.SecurityRoles.Controllers",

                DisplayName = "Security Roles",

                IconClass = "fa fa-shield",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("654f660d-9c71-48f9-8237-593a39a0dbc0"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("7154eb95-36cc-488e-8d24-83b60f3ffffa"),

                ActionName = "ContentTypes",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.ContentManagement.Controllers",

                DisplayName = "Content Types",

                IconClass = "fa fa-th",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("37ec5283-1fec-4779-bd43-9718c5648ffb"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.UserManagement.Controllers",

                DisplayName = "User Management",

                IconClass = "fa fa-users",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("0c30609d-87f3-4d84-9269-cfba91e5c0b6"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("8771beb5-0603-4a81-89bc-9e609f716005"),

                ActionName = "Index",

                ControllerName = "Edit",

                ControllerNamespace = "Deviser.Modules.Security.Controllers",

                DisplayName = "Login Edit",

                IconClass = null,

                IconImage = null,

                IsDefault = false,

                ModuleActionTypeId = Guid.Parse("192278b6-7bf2-40c2-a776-b9ca5fb04fbb"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("4d3d7174-fc7a-4103-9f1a-ac6fc2610819"),

                ActionName = "OptionList",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.ContentManagement.Controllers",

                DisplayName = "Property Option List",

                IconClass = "fa fa-th-list",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("83998364-707b-49ef-abed-b01f864bfe4a"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.PageManagement.Controllers",

                DisplayName = "Page Management",

                IconClass = "fa fa-file-o",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("57813091-da9f-47e3-9d63-dd5c4df79f1d"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("22d7f353-68c6-4c80-b261-c4d21b942623"),

                ActionName = "Login",

                ControllerName = "Account",

                ControllerNamespace = "Deviser.Modules.Security.Controllers",

                DisplayName = "Login",

                IconClass = "fa fa-sign-in",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("d4508962-b521-4e52-ac52-e2bcc06dadd5"),

                ActionName = "Properties",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.ContentManagement.Controllers",

                DisplayName = "Properties ",

                IconClass = "fa fa-sliders",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),
            });

            _dbContext.Set<ModuleAction>
            ().Add(new ModuleAction
            {

                Id = Guid.Parse("5601b5eb-230f-4a43-a906-fed2923aca74"),

                ActionName = "Index",

                ControllerName = "Home",

                ControllerNamespace = "Deviser.Modules.Language.Controllers",

                DisplayName = "Language",

                IconClass = "fa fa-language",

                IconImage = null,

                IsDefault = true,

                ModuleActionTypeId = Guid.Parse("72366792-3740-4e6b-b960-9c9c5334163a"),

                ModuleId = Guid.Parse("73829a91-4a4a-4c22-885a-fb1215e37fdc"),
            });

            //ModuleActionProperty

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("789115a9-2b18-474d-0721-08d58e3bfd70"),
            });

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("1acf1470-d421-4b91-0722-08d58e3bfd70"),
            });

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("16485fed-208b-456a-fd02-08d590dbe247"),
            });

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("217fbbe9-bf8e-4c49-c720-08d5a94c6f03"),
            });

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f"),
            });

            _dbContext.Set<ModuleActionProperty>
            ().Add(new ModuleActionProperty
            {

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                PropertyId = Guid.Parse("aa417aea-c1f2-467c-338d-08d5a95294ba"),
            });

            //Page

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("c597d915-38e0-4c32-0615-08d3a367fbcc"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("56b72d88-5922-4635-0616-08d3a367fbcc"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("8efd99d2-5004-44c6-0617-08d3a367fbcc"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("56ff05c4-57f6-429c-c4ad-08d3a6adbc78"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("20d1b105-5c6d-4961-c4ae-08d3a6adbc78"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("9333f6a1-ba81-4b18-a922-08d3adc0bb30"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("7dff05ab-1376-4ae6-09f0-08d3ae5877da"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("bb858c11-6779-406d-e941-08d3b4c8ff40"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("57942b7c-42a8-405e-aa52-08d3b8ab87fd"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("1322cf31-fae5-40de-d7b7-08d3bfd5ca3d"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                IsDeleted = false,

                IsIncludedInMenu = false,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("2624f356-ff17-49b4-9d18-08d52ace7d21"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("3b11b771-88ce-4755-c1b8-08d57d5f48f2"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("4c099d48-8810-42e2-53d4-08d5869fb686"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("0c1a1106-6d75-4349-53d5-08d5869fb686"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("aaa7c6b9-03e8-424f-a42e-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("11d8379e-20bc-4824-a430-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("d5931c29-0761-4791-a431-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = false,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("4e5b71d2-c764-4ad8-a432-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("5a1b5cf5-adca-4de8-a433-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("bcd8f34d-0a61-4291-a436-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("c015c363-3973-4619-a437-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("81b0f35b-6c3b-4068-a438-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("f5ecb954-c5cc-414b-a439-08d587845981"),

                IsDeleted = true,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("fc6670ef-ab01-4bd2-a43a-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("d82459b7-58ad-4aef-a43b-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("44945c5e-9e39-4cc6-a43c-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("842dd9b2-3cbb-4049-a43d-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("44e735ac-3321-404b-a43e-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("539b960c-be4f-46eb-a43f-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("577d9ed0-a809-4696-a440-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("b7e8dc81-333e-4d35-a441-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("d82d4a2b-9899-4bdc-a442-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("847c808f-a597-4420-a443-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("13b9fc1c-bd49-4123-a444-08d587845981"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                IsDeleted = false,

                IsIncludedInMenu = false,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("dd650840-0ee7-46cf-abb5-8a1591dc0936"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("d1c11d1f-2345-43b6-baa1-8ce890242397"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                IsDeleted = false,

                IsIncludedInMenu = false,

                IsSystem = false,

                ThemeSrc = "[G]Themes/Skyline/Default.cshtml",
            });

            _dbContext.Set<Page>
            ().Add(new Page
            {

                Id = Guid.Parse("c6dd6902-4a9c-4a38-8a05-febe76694993"),

                IsDeleted = false,

                IsIncludedInMenu = true,

                IsSystem = true,

                ThemeSrc = "[G]Themes/Skyline/Admin.cshtml",
            });

            //PageContent

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("a0335b80-b2cc-a282-ea78-01924eee2513"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                ContentTypeId = Guid.Parse("b2c35761-a953-4bf7-bfb2-d0ea9e63786d"),

                IsDeleted = false,

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("de359e4d-27b2-e75b-f9d9-043a71f03d9b"),

                ContainerId = Guid.Parse("6c61bf58-3dd1-a9c7-db96-c31bec104b62"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("2fcd1f08-42b0-fe77-bf67-0fa01148eef3"),

                ContainerId = Guid.Parse("45c16d69-788e-b85e-c4b8-3a9d3b9d7d7d"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("108d471d-e6ac-a321-c2b0-256a079e90df"),

                ContainerId = Guid.Parse("3b3fe768-5622-a826-643c-3afad26b71af"),

                ContentTypeId = Guid.Parse("69933d62-31ed-481e-be1f-95dfb8210027"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": \"btn btn-outline-primary\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("49ff3342-d09e-8965-07b8-27289395cc7f"),

                ContainerId = Guid.Parse("25e41b6f-8809-f983-edde-0516a89feeb2"),

                ContentTypeId = Guid.Parse("f99a54f8-5704-4bc1-b287-3a930c9ece53"),

                IsDeleted = false,

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("033ab6c7-a8e1-536e-9794-27d0752031c6"),

                ContainerId = Guid.Parse("a43f079d-794a-abaa-67c9-ab4835006d7a"),

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IsDeleted = false,

                PageId = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("5416d100-4f46-c549-914c-29e94f6ee06a"),

                ContainerId = Guid.Parse("a43f079d-794a-abaa-67c9-ab4835006d7a"),

                ContentTypeId = Guid.Parse("8d878db7-c3e2-4c39-b359-bd0e39d87df9"),

                IsDeleted = false,

                PageId = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("3d215aa4-9c5b-a93e-1b7c-2df844467824"),

                ContainerId = Guid.Parse("7bb4d1af-616f-cc0b-00e5-a2564a6bfe20"),

                ContentTypeId = Guid.Parse("817fea8f-59e2-4b77-8e63-1ea002772893"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"cssclass\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("64bf349e-df8e-f341-22d4-2e03fbaafa2a"),

                ContainerId = Guid.Parse("26e7b693-7dff-d941-4492-b313b05fef6e"),

                ContentTypeId = Guid.Parse("a3e319ea-80b9-4800-9032-bb7ea09ed331"),

                IsDeleted = false,

                PageId = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("b04a9f79-17a8-b1ff-e5f6-32b81e3936fd"),

                ContainerId = Guid.Parse("a4f55be5-c300-45f5-5412-7e8e5c73b0ae"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("58f3b967-ecf6-3870-46e4-3c0e1d70fe21"),

                ContainerId = Guid.Parse("abe9b84e-c757-885b-8831-eb13ca2350f6"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("aa12bc36-d23d-2b3a-7cdd-48e414795ea5"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IsDeleted = true,

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("2eb17f68-bf88-0767-46e6-4d7e26b3cbaa"),

                ContainerId = Guid.Parse("e75bcaff-78fe-4250-323a-9c7f30786548"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("385b16f5-4350-ff0d-baa6-5e0ad96ea608"),

                ContainerId = Guid.Parse("77ac7c1a-b31a-0a4c-72a8-aa563c318288"),

                ContentTypeId = Guid.Parse("978bd890-7dbd-4ee0-9d86-8356dfadf4e6"),

                IsDeleted = false,

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("a032e37a-ac20-22e4-81af-72faed1cc13c"),

                ContainerId = Guid.Parse("6c61bf58-3dd1-a9c7-db96-c31bec104b62"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = false,

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": \"954d54e1-5c95-da20-422e-9c31691631b2\",\r\n    \"defaultValue\": \"954d54e1-5c95-da20-422e-9c31691631b2\",\r\n    \"description\": \"Could be 'horizontal' or 'vertical' (for vertical slider).\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": \"300\",\r\n    \"defaultValue\": \"300\",\r\n    \"description\": \"Duration of transition between slides (in ms)\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Set to true and slider wrapper will adopt its height to the height of the currently active slide\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": \"20e770b7-b245-16e6-925c-1ce3a036d1ae\",\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Set to true to round values of slides width and height to prevent blurry texts on usual resolution screens (if you have such)\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": \"196f5ee0-c955-7d7a-618d-27863e379a19\",\r\n    \"defaultValue\": \"196f5ee0-c955-7d7a-618d-27863e379a19\",\r\n    \"description\": \"Tranisition effect. Could be \\\"slide\\\", \\\"fade\\\", \\\"cube\\\", \\\"coverflow\\\" or \\\"flip\\\"\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": \"20e770b7-b245-16e6-925c-1ce3a036d1ae\",\r\n    \"defaultValue\": \"20e770b7-b245-16e6-925c-1ce3a036d1ae\",\r\n    \"description\": \"Enable/Disable navigation\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": \"20e770b7-b245-16e6-925c-1ce3a036d1ae\",\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Enable/Disable pagination\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": \"0a50497a-f3d8-53de-932d-2b30a390b125\",\r\n    \"defaultValue\": \"0a50497a-f3d8-53de-932d-2b30a390b125\",\r\n    \"description\": \"String with type of pagination. Can be \\\"bullets\\\", \\\"fraction\\\", \\\"progressbar\\\" or \\\"custom\\\"\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Enable/Disable scrollbar\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"0\",\r\n    \"description\": \"Distance between slides in px.\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"1\",\r\n    \"description\": \"Number of slides per view (slides visible at the same time on slider's container).\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Set to true to enable continuous loop mode\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Enable/Disable keyboard control\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"13916b6e-9c6e-accb-c457-9d71c32909c0\",\r\n    \"description\": \"Enable/Disable mousewheel\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = "Slider 01",
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("3ae46227-d6dc-b5da-9a31-87494a1ee576"),

                ContainerId = Guid.Parse("c7d1df63-d43c-9f98-2f63-22b48f1b4735"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": \"feature-heading\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = "Rich Text 1",
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("ae230727-4341-a44e-6e4a-8bdf6c63ab11"),

                ContainerId = Guid.Parse("0e017a7e-f347-af39-1319-78e9f2bc46eb"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("c9916a98-17b6-f120-f38b-9a028cff7382"),

                ContainerId = Guid.Parse("bc07a679-0c15-6070-98dd-f0815fd36697"),

                ContentTypeId = Guid.Parse("69933d62-31ed-481e-be1f-95dfb8210027"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": \"btn btn-dark\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("4c510763-323a-7992-2000-a62e9db16efe"),

                ContainerId = Guid.Parse("6c61bf58-3dd1-a9c7-db96-c31bec104b62"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("deae8c01-cc70-ebc1-b165-af94417eae61"),

                ContainerId = Guid.Parse("0e017a7e-f347-af39-1319-78e9f2bc46eb"),

                ContentTypeId = Guid.Parse("817fea8f-59e2-4b77-8e63-1ea002772893"),

                IsDeleted = false,

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"cssclass\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("139feaa4-818d-9f1c-d007-c0d304d6d779"),

                ContainerId = Guid.Parse("6c61bf58-3dd1-a9c7-db96-c31bec104b62"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("f32d1048-3a76-50d5-49ea-c8f5499ce67b"),

                ContainerId = Guid.Parse("0e017a7e-f347-af39-1319-78e9f2bc46eb"),

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("2281e807-cf0c-67cb-796e-d41a83761206"),

                ContainerId = Guid.Parse("9e25d404-e1b9-9ea8-9687-0f0bf294cd5e"),

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                IsDeleted = false,

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"fa1b0856-cea0-4245-e3f1-08d582dfee42\",\r\n    \"name\": \"video_preview\",\r\n    \"label\": \"Video Preview\",\r\n    \"value\": \"a935f434-6bb4-5c6a-2889-1216180770bf\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = "Video 01",
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("ee52a5c6-5117-5312-59cf-e43d398fd95e"),

                ContainerId = Guid.Parse("40531ef2-a872-a56a-c06e-8f4bd93cce53"),

                ContentTypeId = Guid.Parse("a7bbfc37-b496-4c8f-b481-309ec38fbac0"),

                IsDeleted = false,

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("9d66eb3d-8795-c015-4a29-e4951e84f5a8"),

                ContainerId = Guid.Parse("ff1a557e-662e-d6c1-c124-65ae939cd243"),

                ContentTypeId = Guid.Parse("c49840f4-5a00-4d1d-86b7-7881e3841314"),

                IsDeleted = false,

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("73919984-3161-5dc2-feff-e5f60b663cd4"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IsDeleted = true,

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("4a28bdd5-742c-e5fc-d4f6-ea0b9b99932d"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                ContentTypeId = Guid.Parse("b2c35761-a953-4bf7-bfb2-d0ea9e63786d"),

                IsDeleted = true,

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("42a6b892-6cc9-10ea-322e-edfc5f7f4c36"),

                ContainerId = Guid.Parse("83049e6c-9d21-f5b9-4158-b4dd4aae4b34"),

                ContentTypeId = Guid.Parse("9b2ec6ac-8fdf-4cb5-ae60-90b73a6931fc"),

                IsDeleted = false,

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"fa1b0856-cea0-4245-e3f1-08d582dfee42\",\r\n    \"name\": \"video_preview\",\r\n    \"label\": \"Video Preview\",\r\n    \"value\": \"a935f434-6bb4-5c6a-2889-1216180770bf\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f645a11f-bc8c-4003-0e83-08d5932d4c52\",\r\n    \"name\": \"image_width\",\r\n    \"label\": \"Image Width\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"300\",\r\n    \"description\": \"Image Width\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"cb4b6f18-7267-41c3-0e84-08d5932d4c52\",\r\n    \"name\": \"image_height\",\r\n    \"label\": \"Image Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": \"200\",\r\n    \"description\": \"Image Height\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = "Video 1",
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("318eea62-fc2e-f3f9-fa90-f1b47d1a0b5c"),

                ContainerId = Guid.Parse("a4f55be5-c300-45f5-5412-7e8e5c73b0ae"),

                ContentTypeId = Guid.Parse("00332002-f2c7-401c-b59c-d0181eaf657b"),

                IsDeleted = true,

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("7bdccc5f-3577-b941-6705-f42accff1bb7"),

                ContainerId = Guid.Parse("6c61bf58-3dd1-a9c7-db96-c31bec104b62"),

                ContentTypeId = Guid.Parse("d2e62921-32f5-4c66-a9b3-e5b61d60b193"),

                IsDeleted = true,

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"0d9134a4-4eb6-40a8-0959-08d58bf2fdc6\",\r\n    \"name\": \"swiper_direction\",\r\n    \"label\": \"Direction\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"278c0cc1-5212-4bfa-095a-08d58bf2fdc6\",\r\n    \"name\": \"swiper_speed\",\r\n    \"label\": \"Speed\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"89e09c02-9c55-4721-5531-08d58c515929\",\r\n    \"name\": \"swiper_autoHeight\",\r\n    \"label\": \"Auto Height\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"238b60e4-bcea-4171-5532-08d58c515929\",\r\n    \"name\": \"swiper_roundLengths\",\r\n    \"label\": \"Round Lengths\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c436196b-f19e-4e3b-5535-08d58c515929\",\r\n    \"name\": \"swiper_effect\",\r\n    \"label\": \"Effect\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4a81a097-a694-4db2-5538-08d58c515929\",\r\n    \"name\": \"swiper_navigation\",\r\n    \"label\": \"Navigation\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"338675b6-6ffa-4f2d-5539-08d58c515929\",\r\n    \"name\": \"swiper_pagination\",\r\n    \"label\": \"Pagination\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"7b2d6527-199b-406d-553b-08d58c515929\",\r\n    \"name\": \"swiper_pagination_type\",\r\n    \"label\": \"Pagination Type\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2157ff87-35a5-405c-553c-08d58c515929\",\r\n    \"name\": \"swiper_scrollbar\",\r\n    \"label\": \"Scrollbar\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"4f6bf74f-b4eb-4b66-553d-08d58c515929\",\r\n    \"name\": \"swiper_spaceBetween\",\r\n    \"label\": \"Space Between\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"84ac0204-f84e-4e1c-553e-08d58c515929\",\r\n    \"name\": \"swiper_slidesPerView\",\r\n    \"label\": \"Sliders per view\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"55f585c6-708a-4487-553f-08d58c515929\",\r\n    \"name\": \"swiper_loop\",\r\n    \"label\": \"Loop\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"2c843984-7009-4287-5540-08d58c515929\",\r\n    \"name\": \"swiper_keyboard\",\r\n    \"label\": \"keyboard\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"017c4f30-da51-4d18-5541-08d58c515929\",\r\n    \"name\": \"swiper_mousewheel\",\r\n    \"label\": \"Mouse Wheel\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 1,

                Title = null,
            });

            _dbContext.Set<PageContent>
            ().Add(new PageContent
            {

                Id = Guid.Parse("afae6e02-101e-c76a-ca10-fa65b2dc9377"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                ContentTypeId = Guid.Parse("d8e458b3-daa2-4bc5-90a0-d56e9a78839e"),

                IsDeleted = true,

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                Properties = "[\r\n  {\r\n    \"id\": \"f5031c31-778b-45dd-bd33-eeb2a088d2bc\",\r\n    \"name\": \"css_class\",\r\n    \"label\": \"Css Class\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",

                SortOrder = 3,

                Title = null,
            });

            //UserRole

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("c6206baf-9ae9-42c2-bda9-97adcf6c8afd"),

                RoleId = Guid.Parse("6bf5335f-c44e-4129-8e6f-0ad578f31e2d"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("21485ff9-6651-41c5-9129-e2dcc034ed9c"),

                RoleId = Guid.Parse("6bf5335f-c44e-4129-8e6f-0ad578f31e2d"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("cc6e4b03-5b96-4582-baa5-68ae2592198d"),

                RoleId = Guid.Parse("f1420b89-34e2-4093-bcd5-0bc9c1f9ce94"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("c6206baf-9ae9-42c2-bda9-97adcf6c8afd"),

                RoleId = Guid.Parse("f1420b89-34e2-4093-bcd5-0bc9c1f9ce94"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("cc6e4b03-5b96-4582-baa5-68ae2592198d"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("c6206baf-9ae9-42c2-bda9-97adcf6c8afd"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("cc6e4b03-5b96-4582-baa5-68ae2592198d"),

                RoleId = Guid.Parse("67eb6f56-623b-4d2a-b602-4bed0388995c"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("715cf6d2-f730-45b0-aa7e-2a14b647f3ba"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("cc6e4b03-5b96-4582-baa5-68ae2592198d"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<IdentityUserRole<Guid>>
            ().Add(new IdentityUserRole<Guid>
            {

                UserId = Guid.Parse("21485ff9-6651-41c5-9129-e2dcc034ed9c"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            //ContentPermission

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("56174c60-7d90-4f3e-8e04-0a8dcff07396"),

                PageContentId = Guid.Parse("7bdccc5f-3577-b941-6705-f42accff1bb7"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("068a6cc8-f59d-4936-b28a-0e425c2004fe"),

                PageContentId = Guid.Parse("58f3b967-ecf6-3870-46e4-3c0e1d70fe21"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("8f14ff05-c706-4edf-a303-11d7b64b1893"),

                PageContentId = Guid.Parse("9d66eb3d-8795-c015-4a29-e4951e84f5a8"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("7c8c366a-3a5a-4a98-b76a-179198ac5148"),

                PageContentId = Guid.Parse("4a28bdd5-742c-e5fc-d4f6-ea0b9b99932d"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("5076774a-d996-48a4-9f4d-1abc6fe9e79f"),

                PageContentId = Guid.Parse("a032e37a-ac20-22e4-81af-72faed1cc13c"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("7e45400b-bceb-42a9-9c8a-1b88dfc49fa6"),

                PageContentId = Guid.Parse("9d66eb3d-8795-c015-4a29-e4951e84f5a8"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("bb9cb1d7-1837-4380-8c24-1e573acf1091"),

                PageContentId = Guid.Parse("64bf349e-df8e-f341-22d4-2e03fbaafa2a"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("4e350c76-d1aa-40b8-b26e-20aeec22a335"),

                PageContentId = Guid.Parse("49ff3342-d09e-8965-07b8-27289395cc7f"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("50e84150-6e68-45d6-9f23-2178df7e2534"),

                PageContentId = Guid.Parse("318eea62-fc2e-f3f9-fa90-f1b47d1a0b5c"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("d2bcd005-95da-41fd-b8d4-23288e779e5e"),

                PageContentId = Guid.Parse("2281e807-cf0c-67cb-796e-d41a83761206"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("d936ec84-d24d-4b26-a9c8-2768b69d4225"),

                PageContentId = Guid.Parse("42a6b892-6cc9-10ea-322e-edfc5f7f4c36"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("94979aca-887d-47a4-92eb-2cae9df0d757"),

                PageContentId = Guid.Parse("3d215aa4-9c5b-a93e-1b7c-2df844467824"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("4e2f2063-2435-4835-8f47-306c6646dbfe"),

                PageContentId = Guid.Parse("afae6e02-101e-c76a-ca10-fa65b2dc9377"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("dddb394e-cbfa-4735-ae25-30f5f382ecff"),

                PageContentId = Guid.Parse("de359e4d-27b2-e75b-f9d9-043a71f03d9b"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("1b1eabbe-6aa7-4e0e-add9-3217cc486761"),

                PageContentId = Guid.Parse("3ae46227-d6dc-b5da-9a31-87494a1ee576"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("37500e74-3423-4b15-95fc-32ef9cdcf381"),

                PageContentId = Guid.Parse("aa12bc36-d23d-2b3a-7cdd-48e414795ea5"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("e9e0a508-e0d5-464e-ad87-33051b00ea83"),

                PageContentId = Guid.Parse("ae230727-4341-a44e-6e4a-8bdf6c63ab11"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("93711825-dcac-41c3-90c3-37459f1e54ee"),

                PageContentId = Guid.Parse("deae8c01-cc70-ebc1-b165-af94417eae61"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("d2b33acb-1cab-4f15-bdee-385ec54deede"),

                PageContentId = Guid.Parse("42a6b892-6cc9-10ea-322e-edfc5f7f4c36"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("1c7e7906-1307-4b71-aa0d-3ab0fb7dc128"),

                PageContentId = Guid.Parse("2281e807-cf0c-67cb-796e-d41a83761206"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("2091bc06-8842-409c-bcfa-3c8afea90e6d"),

                PageContentId = Guid.Parse("ee52a5c6-5117-5312-59cf-e43d398fd95e"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("15a67c1a-8495-4917-910a-40dc36066217"),

                PageContentId = Guid.Parse("f32d1048-3a76-50d5-49ea-c8f5499ce67b"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("ae301e85-935a-4fb8-92da-41c62d9c92b6"),

                PageContentId = Guid.Parse("a0335b80-b2cc-a282-ea78-01924eee2513"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("83b3d144-8447-479f-97c6-4289e0b43dff"),

                PageContentId = Guid.Parse("139feaa4-818d-9f1c-d007-c0d304d6d779"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("117a63d0-3e95-4311-abfb-4ff84dd0c3c6"),

                PageContentId = Guid.Parse("b04a9f79-17a8-b1ff-e5f6-32b81e3936fd"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("8debb813-62e3-4770-bb43-55007fd7a7e0"),

                PageContentId = Guid.Parse("3ae46227-d6dc-b5da-9a31-87494a1ee576"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("a29da18b-8e7d-4c6a-b2b0-5f0bf617345d"),

                PageContentId = Guid.Parse("4c510763-323a-7992-2000-a62e9db16efe"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("bfd8962a-ae16-4c6d-9580-6324899808d9"),

                PageContentId = Guid.Parse("64bf349e-df8e-f341-22d4-2e03fbaafa2a"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("1b993f05-485d-464c-a4ef-66b0110a1b23"),

                PageContentId = Guid.Parse("385b16f5-4350-ff0d-baa6-5e0ad96ea608"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("a2691100-4501-4a30-9de4-6a2500d80fc7"),

                PageContentId = Guid.Parse("033ab6c7-a8e1-536e-9794-27d0752031c6"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("057072b9-313e-4e76-b77b-6c26e0f6af94"),

                PageContentId = Guid.Parse("2eb17f68-bf88-0767-46e6-4d7e26b3cbaa"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("e5109d48-ef7a-49b3-9981-70910c6c7b2e"),

                PageContentId = Guid.Parse("c9916a98-17b6-f120-f38b-9a028cff7382"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("27bcfb4d-368b-46e0-9b31-728464724026"),

                PageContentId = Guid.Parse("2fcd1f08-42b0-fe77-bf67-0fa01148eef3"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("3dcc8813-d5d4-4955-806a-764a7b607dbc"),

                PageContentId = Guid.Parse("afae6e02-101e-c76a-ca10-fa65b2dc9377"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("68a29761-cd77-4924-b207-82823cc1e4bd"),

                PageContentId = Guid.Parse("f32d1048-3a76-50d5-49ea-c8f5499ce67b"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("52df09a3-e9a2-405f-9d90-850d199bf948"),

                PageContentId = Guid.Parse("73919984-3161-5dc2-feff-e5f60b663cd4"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("9309262d-1f37-4bde-9be3-974f94f860cc"),

                PageContentId = Guid.Parse("ae230727-4341-a44e-6e4a-8bdf6c63ab11"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("cea05f09-e7c0-4098-bf03-9970ff4bc0c1"),

                PageContentId = Guid.Parse("a032e37a-ac20-22e4-81af-72faed1cc13c"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("48847f3e-dff7-4ad0-83b7-99c070e99a01"),

                PageContentId = Guid.Parse("4c510763-323a-7992-2000-a62e9db16efe"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("e63318e2-7056-4ea1-be67-9c1affe8f462"),

                PageContentId = Guid.Parse("7bdccc5f-3577-b941-6705-f42accff1bb7"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("dde2a37a-158d-43d9-ad88-9ea6306f1789"),

                PageContentId = Guid.Parse("3d215aa4-9c5b-a93e-1b7c-2df844467824"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("83fca480-d152-48eb-a7d5-a30d85e9dee4"),

                PageContentId = Guid.Parse("c9916a98-17b6-f120-f38b-9a028cff7382"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("cefafe1c-fbb9-402c-be9a-a3743002605a"),

                PageContentId = Guid.Parse("5416d100-4f46-c549-914c-29e94f6ee06a"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("7bae576c-f5df-41ed-abed-a44a5b0bce8b"),

                PageContentId = Guid.Parse("a0335b80-b2cc-a282-ea78-01924eee2513"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("a34261bb-222a-48bd-b05c-a6b0c97abcc4"),

                PageContentId = Guid.Parse("58f3b967-ecf6-3870-46e4-3c0e1d70fe21"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("25b5a0bc-d212-48ac-83bc-b44221b0f88a"),

                PageContentId = Guid.Parse("deae8c01-cc70-ebc1-b165-af94417eae61"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("e14499e4-37e0-41f7-98b2-b57547faf67e"),

                PageContentId = Guid.Parse("2fcd1f08-42b0-fe77-bf67-0fa01148eef3"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("06f10a58-c535-46e5-b497-b68783312dc4"),

                PageContentId = Guid.Parse("033ab6c7-a8e1-536e-9794-27d0752031c6"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("733bea13-95aa-4b7b-8e95-bb0890d81734"),

                PageContentId = Guid.Parse("4a28bdd5-742c-e5fc-d4f6-ea0b9b99932d"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("bd5350cf-23f6-4d0a-9cd3-bcf61a80985d"),

                PageContentId = Guid.Parse("5416d100-4f46-c549-914c-29e94f6ee06a"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("179df18d-3cc1-495d-985f-c47dd3568cf3"),

                PageContentId = Guid.Parse("2eb17f68-bf88-0767-46e6-4d7e26b3cbaa"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("5e0a3ed3-7f81-4591-ad10-d3348373798b"),

                PageContentId = Guid.Parse("b04a9f79-17a8-b1ff-e5f6-32b81e3936fd"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("2790c7ac-f148-4e85-8992-daa49fede46b"),

                PageContentId = Guid.Parse("139feaa4-818d-9f1c-d007-c0d304d6d779"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("800eeca5-0bd9-4ac6-b944-dac374387a8d"),

                PageContentId = Guid.Parse("385b16f5-4350-ff0d-baa6-5e0ad96ea608"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("f9638329-ea1b-4dc9-ba5d-de379e8d7ddf"),

                PageContentId = Guid.Parse("73919984-3161-5dc2-feff-e5f60b663cd4"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("cccd6e5b-1cf2-4c6b-a826-e3db04402b12"),

                PageContentId = Guid.Parse("de359e4d-27b2-e75b-f9d9-043a71f03d9b"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("3412b5be-80d1-4c17-8ae4-e9710065387f"),

                PageContentId = Guid.Parse("318eea62-fc2e-f3f9-fa90-f1b47d1a0b5c"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("c4e0b460-7a3c-4cfe-a02d-ee7db9779ddf"),

                PageContentId = Guid.Parse("49ff3342-d09e-8965-07b8-27289395cc7f"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("fc0e5cdf-cec7-4780-b4f8-f3e1425125bd"),

                PageContentId = Guid.Parse("ee52a5c6-5117-5312-59cf-e43d398fd95e"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("5ebe49f2-5447-4296-8e1c-f7bdd98a5efb"),

                PageContentId = Guid.Parse("aa12bc36-d23d-2b3a-7cdd-48e414795ea5"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("1cd4828d-0350-42c9-81ac-fb870e82931c"),

                PageContentId = Guid.Parse("108d471d-e6ac-a321-c2b0-256a079e90df"),

                PermissionId = Guid.Parse("461b37d9-b801-4235-b74f-0c51f35d170f"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ContentPermission>
            ().Add(new ContentPermission
            {

                Id = Guid.Parse("fa4eeb09-bfa5-4daf-9e57-fbd2e4ee653b"),

                PageContentId = Guid.Parse("108d471d-e6ac-a321-c2b0-256a079e90df"),

                PermissionId = Guid.Parse("491b37a3-deba-4f55-9df6-a67cdd810108"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            //ModulePermission

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("6804b636-e553-4a7e-8b2f-036abbb822a6"),

                PageModuleId = Guid.Parse("540ec70a-f20f-426d-e7e0-cc35dd7b6ddb"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("259f7704-a984-47e1-bd14-090f85aea5c5"),

                PageModuleId = Guid.Parse("7469802f-41f3-e408-1152-6926670ed5ef"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d24ab6b5-9e0f-45e9-bf9d-0ad520219387"),

                PageModuleId = Guid.Parse("33478ee3-1f95-2400-ff46-6a61a58f5b47"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("8f469ad0-76ce-4018-ba6b-0fbc7df054a9"),

                PageModuleId = Guid.Parse("3cf92713-7630-677b-40ba-8aed82edbde6"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("eb301933-4bae-4aac-86e3-0fca1e96a495"),

                PageModuleId = Guid.Parse("a31e9e4d-2a35-b06c-98f1-b5247b85e702"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("bc973740-e807-4eb1-a56f-13a5896faa94"),

                PageModuleId = Guid.Parse("31ab2d4e-5903-9e63-ef5f-f0e48a8c385d"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("04ee95d5-4072-4ce6-9952-15c3ea029564"),

                PageModuleId = Guid.Parse("0e5091c1-0f80-d95e-c213-75238f6e78d9"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("fab131a7-4767-4764-a031-1692c41f85bc"),

                PageModuleId = Guid.Parse("e1ce76d6-04ee-e6db-6855-ab1b697603f5"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("85979bf9-6b33-4ba8-afe0-179ffb27f9f4"),

                PageModuleId = Guid.Parse("7bce8acc-119e-d757-bb12-843a53739721"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("13f734e5-0c85-4c4f-a6e0-18dfa31790cd"),

                PageModuleId = Guid.Parse("31ab2d4e-5903-9e63-ef5f-f0e48a8c385d"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("8fa84ba5-f4be-4d8e-8eec-208d1b7fe66a"),

                PageModuleId = Guid.Parse("8549a29e-646f-d977-486c-494295bdc313"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("f7214bc8-3d9c-4353-94c3-2c535b0e413f"),

                PageModuleId = Guid.Parse("68fab5f5-d7f5-8c84-c122-386d0566b3d3"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("0a23bc68-48af-4611-8067-327c3bfcc6c3"),

                PageModuleId = Guid.Parse("7bce8acc-119e-d757-bb12-843a53739721"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("72c7075c-7150-4bb4-92f7-38e48dbd3e6b"),

                PageModuleId = Guid.Parse("64d8ab17-3f71-d07a-b591-7388e34112bf"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d8bf6163-73b7-419e-b2c6-3eaccf2b9567"),

                PageModuleId = Guid.Parse("b80c3771-21e4-2e81-744e-7bde0e8d5d48"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("cda5927a-2012-4196-ac0b-3efa61a829ba"),

                PageModuleId = Guid.Parse("b8b66733-57a0-6247-e15f-17661be17d8b"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("71f7e0ea-02ee-435a-a683-49ff5c270cb7"),

                PageModuleId = Guid.Parse("dbb6c581-0e72-a160-e0b6-755bf2a949df"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("3f4d1ad0-aa64-426b-aafc-4ac646d4f04e"),

                PageModuleId = Guid.Parse("507f144f-b278-2479-376d-c925187742d9"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("4d9c8f6c-6af8-47de-93b8-4c1098e57738"),

                PageModuleId = Guid.Parse("0b89dd5b-84f3-576c-eab7-27fb306d9941"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("4f4d0321-dad4-4621-b6f9-4d90e74a11d6"),

                PageModuleId = Guid.Parse("b28ea3a7-1f12-df1a-d30a-138193c3afd7"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("97d73334-6eda-48a3-8cf1-4e26caaf75cc"),

                PageModuleId = Guid.Parse("7469802f-41f3-e408-1152-6926670ed5ef"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("add8296d-2f1d-43ef-9961-50546ba80bc3"),

                PageModuleId = Guid.Parse("35349328-976f-4dd0-bf74-57bc523caab8"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("50606c03-0af1-4208-b632-52aefeb8ad9e"),

                PageModuleId = Guid.Parse("b80c3771-21e4-2e81-744e-7bde0e8d5d48"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("3bcd335e-fba2-4687-a2cf-562e2be78a42"),

                PageModuleId = Guid.Parse("b8b66733-57a0-6247-e15f-17661be17d8b"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("99e881be-ed72-4ead-972c-660a250be63f"),

                PageModuleId = Guid.Parse("600c29c9-9471-c20b-13ae-393cb847e956"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("644986a9-bc7c-4f84-8a2d-6a4a8b928f84"),

                PageModuleId = Guid.Parse("64d8ab17-3f71-d07a-b591-7388e34112bf"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("290bdf56-24d2-41e1-b573-6b214f156af4"),

                PageModuleId = Guid.Parse("eab6a316-daa1-66af-423a-a514413e9e1c"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("585173ea-dafd-42cb-919e-7d952ee6ce07"),

                PageModuleId = Guid.Parse("0e5091c1-0f80-d95e-c213-75238f6e78d9"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d3e726df-a9a4-4452-9a63-8351a29b3382"),

                PageModuleId = Guid.Parse("b28ea3a7-1f12-df1a-d30a-138193c3afd7"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("f02afc90-375d-411d-9d94-8b9f4b85220f"),

                PageModuleId = Guid.Parse("a31e9e4d-2a35-b06c-98f1-b5247b85e702"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("f43b918e-d438-47fc-9be9-987f40e4b644"),

                PageModuleId = Guid.Parse("d8ead386-ea3d-ac59-4a6c-5b8e0f0b8680"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("7efc5243-2b93-49e9-81b2-a8c89e1cded5"),

                PageModuleId = Guid.Parse("d8ead386-ea3d-ac59-4a6c-5b8e0f0b8680"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("cebef470-c81d-4070-b76f-b79fa0b47108"),

                PageModuleId = Guid.Parse("507f144f-b278-2479-376d-c925187742d9"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("111e8075-6d7f-4d39-9689-beba9fea9905"),

                PageModuleId = Guid.Parse("cb6a7069-d7e2-e90b-ee35-0a75b74fa717"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("638392a1-05ca-4f77-aaea-c9795dfea462"),

                PageModuleId = Guid.Parse("8549a29e-646f-d977-486c-494295bdc313"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("f28e2a17-1c8e-4ea2-a649-cc21ede84949"),

                PageModuleId = Guid.Parse("600c29c9-9471-c20b-13ae-393cb847e956"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("3691e879-7b7c-45e5-839f-ce3f7b1e9bb8"),

                PageModuleId = Guid.Parse("dbb6c581-0e72-a160-e0b6-755bf2a949df"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("c1f3c057-7a43-4b38-bca9-d453fc208f17"),

                PageModuleId = Guid.Parse("68fab5f5-d7f5-8c84-c122-386d0566b3d3"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("1ab55905-1c4b-4cc8-8d90-d4aac7890ca1"),

                PageModuleId = Guid.Parse("33478ee3-1f95-2400-ff46-6a61a58f5b47"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("252d377a-39d1-4b66-98b2-d999ac5062b3"),

                PageModuleId = Guid.Parse("cb6a7069-d7e2-e90b-ee35-0a75b74fa717"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d5246931-c983-4efb-bd57-dd2fe4bc3069"),

                PageModuleId = Guid.Parse("35349328-976f-4dd0-bf74-57bc523caab8"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("cc19eeae-7b39-42fe-95b1-e5188d01ade2"),

                PageModuleId = Guid.Parse("eab6a316-daa1-66af-423a-a514413e9e1c"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d743061a-565e-4d55-8e86-ecdee8e45204"),

                PageModuleId = Guid.Parse("925d4d7f-2d69-fdd7-e63f-cf0d38b553bc"),

                PermissionId = Guid.Parse("cc3dbe2d-1e4a-46a0-8c10-9e73f1f5c699"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("f7f3b0c9-84fa-4f4d-9595-f459078149b6"),

                PageModuleId = Guid.Parse("925d4d7f-2d69-fdd7-e63f-cf0d38b553bc"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("9071b233-d823-4592-8e56-f6e30af542f3"),

                PageModuleId = Guid.Parse("e1ce76d6-04ee-e6db-6855-ab1b697603f5"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("fc679a46-36a3-4751-84c0-f8377536e590"),

                PageModuleId = Guid.Parse("3cf92713-7630-677b-40ba-8aed82edbde6"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("d6957c79-6d77-4309-912e-f93f9e6be16c"),

                PageModuleId = Guid.Parse("0b89dd5b-84f3-576c-eab7-27fb306d9941"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<ModulePermission>
            ().Add(new ModulePermission
            {

                Id = Guid.Parse("344d7723-ecbc-4a71-a119-ff02d00afcb3"),

                PageModuleId = Guid.Parse("540ec70a-f20f-426d-e7e0-cc35dd7b6ddb"),

                PermissionId = Guid.Parse("34b46847-80be-4099-842a-b654ad550c3e"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            //PageModule

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("cb6a7069-d7e2-e90b-ee35-0a75b74fa717"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = "Contact 2",

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"from\",\r\n    \"label\": \"From\",\r\n    \"value\": \"noreply@deviser.io\",\r\n    \"defaultValue\": \"\",\r\n    \"description\": \"From Address for the Email\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"cf_admin_email\",\r\n    \"label\": \"Admin Email\",\r\n    \"value\": \"sky.karthick@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Contact Form Admin Email Address\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"16485fed-208b-456a-fd02-08d590dbe247\",\r\n    \"name\": \"subject\",\r\n    \"label\": \"Subject\",\r\n    \"value\": \"Test Subject\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Subject for email.\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"217fbbe9-bf8e-4c49-c720-08d5a94c6f03\",\r\n    \"name\": \"cf_view_template\",\r\n    \"label\": \"View Template\",\r\n    \"value\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"defaultValue\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"description\": \"Contact Form View Templates\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f\",\r\n    \"name\": \"cf_admin_email_template\",\r\n    \"label\": \"Admin Email Template\",\r\n    \"value\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"Contact Form Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"aa417aea-c1f2-467c-338d-08d5a95294ba\",\r\n    \"name\": \"cf_contact_email_template\",\r\n    \"label\": \"Contact Email Template\",\r\n    \"value\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"CF Contact Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("b28ea3a7-1f12-df1a-d30a-138193c3afd7"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 3,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("b8b66733-57a0-6247-e15f-17661be17d8b"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("0b89dd5b-84f3-576c-eab7-27fb306d9941"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("68fab5f5-d7f5-8c84-c122-386d0566b3d3"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 1,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": \"sky.karthick@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": \"kowsikanakaraj@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("600c29c9-9471-c20b-13ae-393cb847e956"),

                ContainerId = Guid.Parse("f26bfe84-7ebe-377c-67e3-a889545eaa31"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("22d7f353-68c6-4c80-b261-c4d21b942623"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                SortOrder = 1,

                Title = null,

                Properties = "[]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("378d40e1-403d-5c58-01bc-468fe5bbb9ab"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("54df0a1f-99b0-4847-91f5-7cd187818fe3"),

                ModuleId = Guid.Parse("654f660d-9c71-48f9-8237-593a39a0dbc0"),

                PageId = Guid.Parse("56b72d88-5922-4635-0616-08d3a367fbcc"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("8549a29e-646f-d977-486c-494295bdc313"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": \"th\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": \"dsfhf\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("c1b960f5-7073-ea28-ac32-4ec59bf9e890"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("5601b5eb-230f-4a43-a906-fed2923aca74"),

                ModuleId = Guid.Parse("73829a91-4a4a-4c22-885a-fb1215e37fdc"),

                PageId = Guid.Parse("8efd99d2-5004-44c6-0617-08d3a367fbcc"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("d4f7f41d-d5ef-a619-3e0f-569a0a53ae02"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("57415ac9-9141-495a-a25d-4a80f1c5827a"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),

                PageId = Guid.Parse("20d1b105-5c6d-4961-c4ae-08d3a6adbc78"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("35349328-976f-4dd0-bf74-57bc523caab8"),

                ContainerId = Guid.Parse("f26bfe84-7ebe-377c-67e3-a889545eaa31"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("22d7f353-68c6-4c80-b261-c4d21b942623"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                SortOrder = 1,

                Title = null,

                Properties = "[]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("d8ead386-ea3d-ac59-4a6c-5b8e0f0b8680"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 4,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("fb3942ee-fb96-2aed-03e1-650d814d67d7"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("724a7aa2-4916-40dc-9579-7afc31589d12"),

                ModuleId = Guid.Parse("e99086da-297e-4fdd-a84c-74c663baf9ae"),

                PageId = Guid.Parse("bb858c11-6779-406d-e941-08d3b4c8ff40"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("7469802f-41f3-e408-1152-6926670ed5ef"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = "Contact 2",

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"from\",\r\n    \"label\": \"From\",\r\n    \"value\": \"noreply@deviser.io\",\r\n    \"defaultValue\": \"\",\r\n    \"description\": \"From Address for the Email\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"cf_admin_email\",\r\n    \"label\": \"Admin Email\",\r\n    \"value\": \"sky.karthick@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Contact Form Admin Email Address\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"16485fed-208b-456a-fd02-08d590dbe247\",\r\n    \"name\": \"subject\",\r\n    \"label\": \"Subject\",\r\n    \"value\": \"New Contact\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Subject for email.\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"217fbbe9-bf8e-4c49-c720-08d5a94c6f03\",\r\n    \"name\": \"cf_view_template\",\r\n    \"label\": \"View Template\",\r\n    \"value\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"defaultValue\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"description\": \"Contact Form View Templates\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f\",\r\n    \"name\": \"cf_admin_email_template\",\r\n    \"label\": \"Admin Email Template\",\r\n    \"value\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"Contact Form Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"aa417aea-c1f2-467c-338d-08d5a95294ba\",\r\n    \"name\": \"cf_contact_email_template\",\r\n    \"label\": \"Contact Email Template\",\r\n    \"value\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"CF Contact Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("33478ee3-1f95-2400-ff46-6a61a58f5b47"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("64d8ab17-3f71-d07a-b591-7388e34112bf"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 3,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("0e5091c1-0f80-d95e-c213-75238f6e78d9"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 3,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("dbb6c581-0e72-a160-e0b6-755bf2a949df"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 1,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("b80c3771-21e4-2e81-744e-7bde0e8d5d48"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": \"tz\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": \"rt\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("7bce8acc-119e-d757-bb12-843a53739721"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("3cf92713-7630-677b-40ba-8aed82edbde6"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 4,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("f5c81ac0-bebb-ebb3-e666-9a47112ae346"),

                ContainerId = Guid.Parse("f26bfe84-7ebe-377c-67e3-a889545eaa31"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("7f2e81f9-90c3-4247-a545-658cc370caf5"),

                ModuleId = Guid.Parse("e4792855-5df8-4186-ad32-69d6464c748f"),

                PageId = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("ad5d1f7a-3a44-dddd-04a1-9a77a5f0f8b3"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("4d3d7174-fc7a-4103-9f1a-ac6fc2610819"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),

                PageId = Guid.Parse("9333f6a1-ba81-4b18-a922-08d3adc0bb30"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("eab6a316-daa1-66af-423a-a514413e9e1c"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 4,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("e1ce76d6-04ee-e6db-6855-ab1b697603f5"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("3bc79404-700a-47e1-ca1f-08d52ace68d7"),

                ModuleId = Guid.Parse("d670ac96-2ab6-4036-4664-08d52acdf1a1"),

                PageId = Guid.Parse("2624f356-ff17-49b4-9d18-08d52ace7d21"),

                SortOrder = 1,

                Title = "Recycle Bin 04",

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("a31e9e4d-2a35-b06c-98f1-b5247b85e702"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 4,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": \"xscyxc\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": \"cxycx\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("f9d2f198-e489-6533-00f7-c25d8d920fee"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("37ec5283-1fec-4779-bd43-9718c5648ffb"),

                ModuleId = Guid.Parse("0c30609d-87f3-4d84-9269-cfba91e5c0b6"),

                PageId = Guid.Parse("c597d915-38e0-4c32-0615-08d3a367fbcc"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("beb22251-396f-4e69-91ab-c28aa93f7bde"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("83998364-707b-49ef-abed-b01f864bfe4a"),

                ModuleId = Guid.Parse("57813091-da9f-47e3-9d63-dd5c4df79f1d"),

                PageId = Guid.Parse("c6dd6902-4a9c-4a38-8a05-febe76694993"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("507f144f-b278-2479-376d-c925187742d9"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 2,

                Title = "Contact 2",

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"from\",\r\n    \"label\": \"From\",\r\n    \"value\": \"kowsikarthick88@gmail.com\",\r\n    \"defaultValue\": \"\",\r\n    \"description\": \"From Address for the Email\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"cf_admin_email\",\r\n    \"label\": \"Admin Email\",\r\n    \"value\": \"kowsikanakaraj@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Contact Form Admin Email Address\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"16485fed-208b-456a-fd02-08d590dbe247\",\r\n    \"name\": \"subject\",\r\n    \"label\": \"Subject\",\r\n    \"value\": \"Contact - Query From User\",\r\n    \"defaultValue\": null,\r\n    \"description\": \"Subject for email.\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"217fbbe9-bf8e-4c49-c720-08d5a94c6f03\",\r\n    \"name\": \"cf_view_template\",\r\n    \"label\": \"View Template\",\r\n    \"value\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"defaultValue\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"description\": \"Contact Form View Templates\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f\",\r\n    \"name\": \"cf_admin_email_template\",\r\n    \"label\": \"Admin Email Template\",\r\n    \"value\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"Contact Form Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"aa417aea-c1f2-467c-338d-08d5a95294ba\",\r\n    \"name\": \"cf_contact_email_template\",\r\n    \"label\": \"Contact Email Template\",\r\n    \"value\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"defaultValue\": \"384df06f-63cd-effb-faf8-b152885ba305\",\r\n    \"description\": \"CF Contact Email Template\",\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("540ec70a-f20f-426d-e7e0-cc35dd7b6ddb"),

                ContainerId = Guid.Parse("f26bfe84-7ebe-377c-67e3-a889545eaa31"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                SortOrder = 1,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"from\",\r\n    \"label\": \"From\",\r\n    \"value\": \"sky.karthick@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"cf_admin_email\",\r\n    \"label\": \"Admin Email\",\r\n    \"value\": \"sky.karthick@gmail.com\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"16485fed-208b-456a-fd02-08d590dbe247\",\r\n    \"name\": \"subject\",\r\n    \"label\": \"Subject\",\r\n    \"value\": \"New Contact\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"217fbbe9-bf8e-4c49-c720-08d5a94c6f03\",\r\n    \"name\": \"cf_view_template\",\r\n    \"label\": \"View Template\",\r\n    \"value\": \"8f9ccd68-101d-cc14-ee4a-2676aaedc3f5\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"c1a87a70-1b7a-4fe6-09bc-08d5a94f7b1f\",\r\n    \"name\": \"cf_admin_email_template\",\r\n    \"label\": \"Admin Email Template\",\r\n    \"value\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"aa417aea-c1f2-467c-338d-08d5a95294ba\",\r\n    \"name\": \"cf_contact_email_template\",\r\n    \"label\": \"Contact Email Template\",\r\n    \"value\": \"eb2b86b0-20a3-e218-d6d4-d82b448dc778\",\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("925d4d7f-2d69-fdd7-e63f-cf0d38b553bc"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 1,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("af0ea3e1-de98-8749-58d6-e0f95cc1f061"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("ae7afca8-56f6-4381-822c-1a04022c779b"),

                ModuleId = Guid.Parse("f271f063-aa57-4ee0-95a4-d1417fab15c4"),

                PageId = Guid.Parse("1322cf31-fae5-40de-d7b7-08d3bfd5ca3d"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("720785c4-b947-ee8e-a835-ea1fa95b1c30"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("7154eb95-36cc-488e-8d24-83b60f3ffffa"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),

                PageId = Guid.Parse("56ff05c4-57f6-429c-c4ad-08d3a6adbc78"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("31ab2d4e-5903-9e63-ef5f-f0e48a8c385d"),

                ContainerId = Guid.Parse("0fcf04a2-3d71-26b0-c371-6d936c6c65d8"),

                IsDeleted = true,

                ModuleActionId = Guid.Parse("9994b49e-7012-4a02-e1c7-08d56a4703c5"),

                ModuleId = Guid.Parse("c75b54cc-8e9d-42cc-f1e8-08d568c7a843"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                SortOrder = 1,

                Title = null,

                Properties = "[\r\n  {\r\n    \"id\": \"789115a9-2b18-474d-0721-08d58e3bfd70\",\r\n    \"name\": \"From\",\r\n    \"label\": \"From\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  },\r\n  {\r\n    \"id\": \"1acf1470-d421-4b91-0722-08d58e3bfd70\",\r\n    \"name\": \"To\",\r\n    \"label\": \"To\",\r\n    \"value\": null,\r\n    \"defaultValue\": null,\r\n    \"description\": null,\r\n    \"optionListId\": null,\r\n    \"optionList\": null,\r\n    \"isActive\": false,\r\n    \"createdDate\": null,\r\n    \"lastModifiedDate\": null\r\n  }\r\n]",
            });

            _dbContext.Set<PageModule>
            ().Add(new PageModule
            {

                Id = Guid.Parse("c0de3816-435b-397c-3b3d-f310c9b5a0b7"),

                ContainerId = Guid.Parse("a2b3cf83-2533-27f9-b8fc-843681daa777"),

                IsDeleted = false,

                ModuleActionId = Guid.Parse("d4508962-b521-4e52-ac52-e2bcc06dadd5"),

                ModuleId = Guid.Parse("f32fa4c5-d319-48b0-a68b-cffb9c8743d5"),

                PageId = Guid.Parse("7dff05ab-1376-4ae6-09f0-08d3ae5877da"),

                SortOrder = 1,

                Title = null,

                Properties = null,
            });

            //PagePermission

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("560ab974-0ec0-48a8-ada1-0415ffeccc3f"),

                PageId = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("67612711-bda4-44bb-aa5f-05dd347af16f"),

                PageId = Guid.Parse("842dd9b2-3cbb-4049-a43d-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("5c9d80d5-093d-4617-71e3-08d3a8de8f2d"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("0805288f-1a1e-4c02-71e4-08d3a8de8f2d"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ebf06e8a-9a5b-4128-4969-08d3a8df5ecd"),

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e3e709ea-9e5c-4fc8-496a-08d3a8df5ecd"),

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("4bd1bfd3-aec4-4360-496b-08d3a8df5ecd"),

                PageId = Guid.Parse("dd650840-0ee7-46cf-abb5-8a1591dc0936"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2d4d02a0-566d-480e-496c-08d3a8df5ecd"),

                PageId = Guid.Parse("dd650840-0ee7-46cf-abb5-8a1591dc0936"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("5171e293-5dbc-4c73-496d-08d3a8df5ecd"),

                PageId = Guid.Parse("c6dd6902-4a9c-4a38-8a05-febe76694993"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("fcd8658d-8953-48f0-496e-08d3a8df5ecd"),

                PageId = Guid.Parse("c6dd6902-4a9c-4a38-8a05-febe76694993"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2682dc85-bbb4-4d47-496f-08d3a8df5ecd"),

                PageId = Guid.Parse("c597d915-38e0-4c32-0615-08d3a367fbcc"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("578f98cc-7615-4ee3-4970-08d3a8df5ecd"),

                PageId = Guid.Parse("c597d915-38e0-4c32-0615-08d3a367fbcc"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("551b1da9-2660-467b-4971-08d3a8df5ecd"),

                PageId = Guid.Parse("56b72d88-5922-4635-0616-08d3a367fbcc"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("3bb1da62-8a37-493c-4972-08d3a8df5ecd"),

                PageId = Guid.Parse("56b72d88-5922-4635-0616-08d3a367fbcc"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("17b46d3b-8a9a-459e-4973-08d3a8df5ecd"),

                PageId = Guid.Parse("8efd99d2-5004-44c6-0617-08d3a367fbcc"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("20b29761-91ca-4b3b-4974-08d3a8df5ecd"),

                PageId = Guid.Parse("8efd99d2-5004-44c6-0617-08d3a367fbcc"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("128ef23c-ad6b-4256-4975-08d3a8df5ecd"),

                PageId = Guid.Parse("20d1b105-5c6d-4961-c4ae-08d3a6adbc78"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("fba49043-bf78-4ab2-4976-08d3a8df5ecd"),

                PageId = Guid.Parse("20d1b105-5c6d-4961-c4ae-08d3a6adbc78"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e88de8fc-92c9-4ec2-4977-08d3a8df5ecd"),

                PageId = Guid.Parse("56ff05c4-57f6-429c-c4ad-08d3a6adbc78"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("debe9797-540a-47ef-4978-08d3a8df5ecd"),

                PageId = Guid.Parse("56ff05c4-57f6-429c-c4ad-08d3a6adbc78"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("f093cfe9-9b41-4076-497a-08d3a8df5ecd"),

                PageId = Guid.Parse("62328d72-ad82-4de2-9a98-c954e5b30b28"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("77440408-f9b6-46dc-90a2-08d5642951dc"),

                PageId = Guid.Parse("d5d5a9fd-511b-4025-b495-8908fb70c762"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ae2878cf-1137-42de-956a-09ad45c5152f"),

                PageId = Guid.Parse("2624f356-ff17-49b4-9d18-08d52ace7d21"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("72d54e8b-2419-4094-9ad2-0cb2b1f33694"),

                PageId = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("9be5cb5c-e169-4f33-9721-0e42b2f0f4a1"),

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6c7a45d6-ac2a-4f20-b61e-137cd7c484c4"),

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c2476016-7365-4343-8240-14a5e25a9910"),

                PageId = Guid.Parse("11d8379e-20bc-4824-a430-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("25dbdcb6-524e-4f79-8585-16459c12d6d8"),

                PageId = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d8d7c196-460d-4b9e-ba30-19e929c596f9"),

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("79af2fb6-26d9-4fe9-8a21-2081344e68ed"),

                PageId = Guid.Parse("44e735ac-3321-404b-a43e-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c534d920-9096-4a7c-bff2-2095e51ccab1"),

                PageId = Guid.Parse("b7e8dc81-333e-4d35-a441-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("0960154c-a3dc-4d00-9e07-21289d3d9c12"),

                PageId = Guid.Parse("57942b7c-42a8-405e-aa52-08d3b8ab87fd"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("301b5074-1994-4cfb-aefd-26114113092a"),

                PageId = Guid.Parse("c015c363-3973-4619-a437-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("bf76f950-a2c9-4021-9b77-2641be0e986b"),

                PageId = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("82afd7e4-789f-4682-8921-279c8d9db7f8"),

                PageId = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("dab0020b-0751-446e-93fe-2c8ebbe0eba6"),

                PageId = Guid.Parse("11d8379e-20bc-4824-a430-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("da0eb66e-8854-4ee2-bbfe-2e6b1be7d45c"),

                PageId = Guid.Parse("5a1b5cf5-adca-4de8-a433-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c2738ba6-afae-4b42-997f-2f1a11083604"),

                PageId = Guid.Parse("7dff05ab-1376-4ae6-09f0-08d3ae5877da"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("8581267c-4dd2-4d53-9de5-2f3107765cf2"),

                PageId = Guid.Parse("fc6670ef-ab01-4bd2-a43a-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d95fabbc-74a5-4628-873c-2f3afb2850de"),

                PageId = Guid.Parse("3b11b771-88ce-4755-c1b8-08d57d5f48f2"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("7bbfce1c-f5d2-4ee6-b2b6-32f5e389ed5f"),

                PageId = Guid.Parse("4e5b71d2-c764-4ad8-a432-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e9656327-3622-40b8-b26a-357bd66caca6"),

                PageId = Guid.Parse("51a79e31-9bb1-4fa7-4da6-08d3c2d166ce"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ad783456-220d-4e06-a00b-3613fc707f73"),

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ebb3d28f-b251-46cf-b60e-3732e7a72a05"),

                PageId = Guid.Parse("4e5b71d2-c764-4ad8-a432-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("7545ef25-a515-4b53-b7b5-39c8e20fda1c"),

                PageId = Guid.Parse("fc6670ef-ab01-4bd2-a43a-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("0658bc26-1a52-480d-97a8-3b8a910df6e2"),

                PageId = Guid.Parse("44945c5e-9e39-4cc6-a43c-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c66a874f-978c-49a1-af6c-3cee553c45b4"),

                PageId = Guid.Parse("bb858c11-6779-406d-e941-08d3b4c8ff40"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("8e57da41-a1a3-410e-a114-3e261d742e05"),

                PageId = Guid.Parse("f5ecb954-c5cc-414b-a439-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("4e006339-df82-48b7-af0c-413c8a114ba5"),

                PageId = Guid.Parse("aaa7c6b9-03e8-424f-a42e-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("7ab9eb2f-1cc9-447f-a777-43426c0f95e7"),

                PageId = Guid.Parse("bcd8f34d-0a61-4291-a436-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d371c14c-16ed-4798-ac37-4fd437f70140"),

                PageId = Guid.Parse("7dff05ab-1376-4ae6-09f0-08d3ae5877da"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("da065106-9c02-4408-a954-51107f1a699c"),

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("89bbd847-95e1-4e9f-8abe-53d9553d795f"),

                PageId = Guid.Parse("5a1b5cf5-adca-4de8-a433-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("702aae0a-59f7-456b-aa5f-57d814bf77f3"),

                PageId = Guid.Parse("5a1b5cf5-adca-4de8-a433-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("1b5ba80b-e714-42b1-96a0-5a20d750ba94"),

                PageId = Guid.Parse("9333f6a1-ba81-4b18-a922-08d3adc0bb30"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("a63aac51-c374-4a10-a1da-5a6e846dd540"),

                PageId = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("5edb6858-5e35-4ce5-bf24-5d1fe268cd36"),

                PageId = Guid.Parse("0c1a1106-6d75-4349-53d5-08d5869fb686"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e66d9ec1-eac0-4856-b1e7-5fb2ccc9bc35"),

                PageId = Guid.Parse("bcd8f34d-0a61-4291-a436-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("588310b8-7da4-42e5-b45a-614d08882471"),

                PageId = Guid.Parse("0c1a1106-6d75-4349-53d5-08d5869fb686"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d7284668-5574-4998-953c-6e54e9a37e10"),

                PageId = Guid.Parse("d5931c29-0761-4791-a431-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("75dda2ee-ea80-4735-9ce2-6f4892a34bd2"),

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("5db3fbca-f818-43cd-88c6-7c7a1968e574"),

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2205f52a-d129-4789-aebd-7db1e9974c99"),

                PageId = Guid.Parse("3b11b771-88ce-4755-c1b8-08d57d5f48f2"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("147dad38-af4c-4ec4-9dc1-81665b8b3375"),

                PageId = Guid.Parse("81b0f35b-6c3b-4068-a438-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("63186337-b352-4761-851c-8533c8c5fb5d"),

                PageId = Guid.Parse("577d9ed0-a809-4696-a440-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c6b06a8a-a613-4bd1-86d8-8bd0518e0cb0"),

                PageId = Guid.Parse("d82459b7-58ad-4aef-a43b-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("29775db4-011e-4bc1-938c-8d68d62db840"),

                PageId = Guid.Parse("7505e6d3-bb44-41bb-67ee-08d5a2c8b666"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6312f583-39c2-4740-ac47-8d7a1e668192"),

                PageId = Guid.Parse("81b0f35b-6c3b-4068-a438-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e25993f8-82e2-4a69-bf41-927706fb55de"),

                PageId = Guid.Parse("2624f356-ff17-49b4-9d18-08d52ace7d21"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("fb244530-aeb5-4adc-8e2b-9ad168b38737"),

                PageId = Guid.Parse("4c099d48-8810-42e2-53d4-08d5869fb686"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6cced069-4a71-4893-9d54-9ad3a05e8b71"),

                PageId = Guid.Parse("4c099d48-8810-42e2-53d4-08d5869fb686"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2f6c166c-7cf5-4afc-b3be-9caab4ed76c4"),

                PageId = Guid.Parse("847c808f-a597-4420-a443-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ea670fa8-24ec-4f84-91a5-9e8aa8f2cbc5"),

                PageId = Guid.Parse("539b960c-be4f-46eb-a43f-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("49bc51bf-ca51-442a-9d18-9eda26c80cb8"),

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("86538898-af0f-4b3d-b703-a3ec17bf82ce"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("aac779cb-4e1e-47e8-a90e-a407f9502bce"),

                PageId = Guid.Parse("aaa7c6b9-03e8-424f-a42e-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("4e0ec130-9451-4b66-a23b-a4bb5c0e7070"),

                PageId = Guid.Parse("d82d4a2b-9899-4bdc-a442-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d9dac7d7-4246-4f01-abae-a757cd22e40b"),

                PageId = Guid.Parse("577d9ed0-a809-4696-a440-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("7d1b14bb-ce02-4d8f-ae26-a820457be867"),

                PageId = Guid.Parse("57942b7c-42a8-405e-aa52-08d3b8ab87fd"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2bdfe57f-22b6-47a0-8b0e-a935037cc0f2"),

                PageId = Guid.Parse("42e0d3c9-2269-46fd-a42a-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("5dd6c8e1-cc63-45f4-83a7-abd2ae8c4c94"),

                PageId = Guid.Parse("4c099d48-8810-42e2-53d4-08d5869fb686"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("aa52f854-eab1-4c6e-9a3d-afc916e5032f"),

                PageId = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6c0b3ccc-bedf-4c14-af0f-b1e5d9b360a4"),

                PageId = Guid.Parse("4e5b71d2-c764-4ad8-a432-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("b5183e29-d054-48fe-8bc0-b208394e72a1"),

                PageId = Guid.Parse("d5931c29-0761-4791-a431-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("2bcbf45d-6bca-4338-b310-b306c5691b84"),

                PageId = Guid.Parse("d5931c29-0761-4791-a431-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("efd5e92e-212d-46fe-a756-b495ebad0e35"),

                PageId = Guid.Parse("b7e8dc81-333e-4d35-a441-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("df13891c-a963-42c6-9bb5-b70981ec7506"),

                PageId = Guid.Parse("c015c363-3973-4619-a437-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("237f7d24-111d-41af-80a8-b83b4240bb7e"),

                PageId = Guid.Parse("f5ecb954-c5cc-414b-a439-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("8251c2d8-da3b-44ab-9a68-bb6afc1856b6"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("11840b85-bcca-4220-9129-c059183ef38e"),

                PageId = Guid.Parse("847c808f-a597-4420-a443-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("f6485df0-43b3-42a7-8acf-c3bf418c7714"),

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("9c01d929-6724-4046-b372-c6386a070b7a"),

                PageId = Guid.Parse("1e2f0a6a-7197-400a-a42b-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("855b9edf-5569-4fe8-8103-c63c469bf5dd"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("a6331233-1eb1-4dc2-8372-cacbe7ace8ae"),

                PageId = Guid.Parse("d82d4a2b-9899-4bdc-a442-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("743bdfb6-eb0c-4bd5-ac1b-cdb8d2ccb702"),

                PageId = Guid.Parse("539b960c-be4f-46eb-a43f-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("976f00ba-d857-48b4-9eb4-ce351bba154e"),

                PageId = Guid.Parse("11d8379e-20bc-4824-a430-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("9ac2622c-61ca-4bc2-992d-d3d9944612f0"),

                PageId = Guid.Parse("1322cf31-fae5-40de-d7b7-08d3bfd5ca3d"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6a6687e6-a077-4a7e-ba0c-d820fcc3713a"),

                PageId = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("4c40ffee-144a-4456-abce-d85f54a9b0dc"),

                PageId = Guid.Parse("aaa7c6b9-03e8-424f-a42e-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("6fd1af97-3b1e-4552-8ba0-da11f7bb1f71"),

                PageId = Guid.Parse("faa9caaa-1fe2-40a9-a435-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("4233b0fa-cf5f-4008-a789-da2bddbe5612"),

                PageId = Guid.Parse("d82459b7-58ad-4aef-a43b-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("3d88810c-332e-4f16-ba93-db19043941c2"),

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("d281405d-adab-4e8d-a0b0-db70f8df4a24"),

                PageId = Guid.Parse("842dd9b2-3cbb-4049-a43d-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c2f9a467-f3e3-4573-b05b-dd252c5ac01a"),

                PageId = Guid.Parse("0c1a1106-6d75-4349-53d5-08d5869fb686"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("318ca625-2d22-4f06-9580-ddbe258ca367"),

                PageId = Guid.Parse("bcd8f34d-0a61-4291-a436-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("fb108f9e-5039-4a3b-bc2d-df5f0a098c05"),

                PageId = Guid.Parse("44945c5e-9e39-4cc6-a43c-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("8c6dc9dc-4563-4010-bc16-e003c9d62e6d"),

                PageId = Guid.Parse("44e735ac-3321-404b-a43e-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("41274253-e447-4c37-92c5-e47ffbf4674a"),

                PageId = Guid.Parse("13b9fc1c-bd49-4123-a444-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("e1270dba-bb94-4ae4-8ff9-e5e3ece36018"),

                PageId = Guid.Parse("19e8e352-d244-4b05-a42f-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("3c9c1431-fc70-4d15-a1f1-e79063e4fc61"),

                PageId = Guid.Parse("3b11b771-88ce-4755-c1b8-08d57d5f48f2"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("ec31847c-d1c8-4a4e-8695-e7ed56d2b69d"),

                PageId = Guid.Parse("8dd34791-fad4-4f9d-a42c-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("fc885496-771a-4b90-b264-ec841df6a9fd"),

                PageId = Guid.Parse("1322cf31-fae5-40de-d7b7-08d3bfd5ca3d"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("c3b2d9e5-c976-4514-9d45-f0224d64a313"),

                PageId = Guid.Parse("bb858c11-6779-406d-e941-08d3b4c8ff40"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("f2c4d011-bef1-479b-9793-f2416f5060f3"),

                PageId = Guid.Parse("5214eb20-b815-499a-a434-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("086357bf-01b1-494c-a8b8-54fdfa7c4c9e"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("f901ab4c-1b11-493f-9212-f79b69fd64da"),

                PageId = Guid.Parse("9333f6a1-ba81-4b18-a922-08d3adc0bb30"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("cda92380-2701-4cb0-99e9-f9a64ef9e569"),

                PageId = Guid.Parse("72eb8147-8171-4d39-a42d-08d587845981"),

                PermissionId = Guid.Parse("29cb1b57-1862-4300-b378-f3271b870148"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("077e9049-98e5-4d9d-bd18-fcab88e0bb75"),

                PageId = Guid.Parse("13b9fc1c-bd49-4123-a444-08d587845981"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.Set<PagePermission>
            ().Add(new PagePermission
            {

                Id = Guid.Parse("b2ae2faa-32c4-41a0-9adb-fd433f4e3874"),

                PageId = Guid.Parse("5e9b5792-f8dd-4852-1a16-08d583b38502"),

                PermissionId = Guid.Parse("2da41181-be15-4ad6-a89c-3ba8b71f993b"),

                RoleId = Guid.Parse("9b461499-c49e-4398-bfed-4364a176ebbd"),
            });

            _dbContext.SaveChanges();
        }
    }
}
