USE [DeviserWI]
GO

INSERT [dbo].[Language] ([Id], [CreatedDate], [CultureCode], [EnglishName], [FallbackCulture], [IsActive], [LastModifiedDate], [NativeName]) VALUES (N'388b34ed-803a-48ad-6618-08d360bcd031', CAST(N'2016-04-09 23:20:37.487' AS DateTime), N'de-CH', N'German (Switzerland)', N'en-US', 1, CAST(N'2016-04-09 23:20:37.487' AS DateTime), N'Deutsch (Schweiz)')
GO
INSERT [dbo].[Language] ([Id], [CreatedDate], [CultureCode], [EnglishName], [FallbackCulture], [IsActive], [LastModifiedDate], [NativeName]) VALUES (N'1350c0b2-634e-4e81-6619-08d360bcd031', CAST(N'2016-04-09 23:24:04.597' AS DateTime), N'fr-CH', N'French (Switzerland)', N'en-US', 1, CAST(N'2016-04-09 23:24:04.597' AS DateTime), N'français (Suisse)')
GO
INSERT [dbo].[Language] ([Id], [CreatedDate], [CultureCode], [EnglishName], [FallbackCulture], [IsActive], [LastModifiedDate], [NativeName]) VALUES (N'4a8a96c4-b125-433a-b0b7-e8ddbcfaa381', NULL, N'en-US', N'English (United States)', N'en-US', 1, NULL, N'English (United States)')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[SiteSetting] ([Id], [SettingName], [SettingValue]) VALUES (N'08c40572-06c6-4c12-932d-a1ba4a36710b', N'HomePageId', N'd5d5a9fd-511b-4025-b495-8908fb70c762')
GO
INSERT [dbo].[SiteSetting] ([Id], [SettingName], [SettingValue]) VALUES (N'2806bd6b-6dec-4385-a568-af20a51d241c', N'DefaultAdminLayoutId', NULL)
GO
INSERT [dbo].[SiteSetting] ([Id], [SettingName], [SettingValue]) VALUES (N'ebd27836-16b2-4135-86ec-cfbd5234f704', N'SiteRoot', N'/')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[ContentDataType] ([Id], [Label], [Name]) VALUES (N'903ac568-10b9-4aba-beca-2d9c5c15d8af', N'Object', N'object')
GO
INSERT [dbo].[ContentDataType] ([Id], [Label], [Name]) VALUES (N'8efbe559-c5b9-4889-a926-49872a11b654', N'Array', N'array')
GO
INSERT [dbo].[ContentDataType] ([Id], [Label], [Name]) VALUES (N'cdcc92ce-48f8-42d0-989c-62f5aacd7dc2', N'String', N'string')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[Property] ([Id], [Label], [Name], [PropertyOptionListId]) VALUES (N'f5031c31-778b-45dd-bd33-eeb2a088d2bc', N'Css Class', N'cssClass', NULL)
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[ContentType] ([Id], [ContentDataTypeId], [IconClass], [IconImage], [Label], [Name], [SortOrder]) VALUES (N'f2e91a21-0864-4b16-b3de-9be08888b91f', N'903ac568-10b9-4aba-beca-2d9c5c15d8af', N'fa fa-picture-o', NULL, N'Image', N'image', 2)
GO
INSERT [dbo].[ContentType] ([Id], [ContentDataTypeId], [IconClass], [IconImage], [Label], [Name], [SortOrder]) VALUES (N'00332002-f2c7-401c-b59c-d0181eaf657b', N'cdcc92ce-48f8-42d0-989c-62f5aacd7dc2', N'fa fa-font', NULL, N'Text', N'text', 1)
GO
INSERT [dbo].[ContentType] ([Id], [ContentDataTypeId], [IconClass], [IconImage], [Label], [Name], [SortOrder]) VALUES (N'd2e62921-32f5-4c66-a9b3-e5b61d60b193', N'8efbe559-c5b9-4889-a926-49872a11b654', N'fa fa-caret-square-o-right', NULL, N'Carousel', N'carousel', 3)
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[ContentTypeProperty] ([Id], [ConentTypeId], [PropertyId]) VALUES (N'571623ec-88cb-46ac-8f35-378f88efbaf9', N'00332002-f2c7-401c-b59c-d0181eaf657b', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
INSERT [dbo].[ContentTypeProperty] ([Id], [ConentTypeId], [PropertyId]) VALUES (N'efa572cd-df11-4cd8-8e3e-83531c4fb759', N'd2e62921-32f5-4c66-a9b3-e5b61d60b193', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
INSERT [dbo].[ContentTypeProperty] ([Id], [ConentTypeId], [PropertyId]) VALUES (N'8071b325-1712-4f55-8741-a7a9bf44491b', N'f2e91a21-0864-4b16-b3de-9be08888b91f', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[LayoutType] ([Id], [IconClass], [IconImage], [Label], [LayoutTypeIds], [Name]) VALUES (N'5a0a5884-da84-4922-a02f-5828b55d5c92', N'fa fa-square-o', NULL, N'Wrapper', N'9341f92e-83d8-4afe-ad4a-a95deeda9ae3, 43734210-943e-4f33-a161-f12260b8c001', N'wrapper')
GO
INSERT [dbo].[LayoutType] ([Id], [IconClass], [IconImage], [Label], [LayoutTypeIds], [Name]) VALUES (N'4c98f160-d676-40a2-9b88-79fd1343f333', N'fa fa-columns', NULL, N'Column', N'9341f92e-83d8-4afe-ad4a-a95deeda9ae3, 43734210-943e-4f33-a161-f12260b8c001', N'column')
GO
INSERT [dbo].[LayoutType] ([Id], [IconClass], [IconImage], [Label], [LayoutTypeIds], [Name]) VALUES (N'9341f92e-83d8-4afe-ad4a-a95deeda9ae3', N'fa fa-square-o', NULL, N'Container', N'9341f92e-83d8-4afe-ad4a-a95deeda9ae3, 43734210-943e-4f33-a161-f12260b8c001', N'container')
GO
INSERT [dbo].[LayoutType] ([Id], [IconClass], [IconImage], [Label], [LayoutTypeIds], [Name]) VALUES (N'43734210-943e-4f33-a161-f12260b8c001', N'fa fa-align-justify', NULL, N'Row', N'4c98f160-d676-40a2-9b88-79fd1343f333', N'row')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[LayoutTypeProperty] ([Id], [LayoutTypeId], [PropertyId]) VALUES (N'3d1a07ce-fa7f-4eea-93e7-a39c929f7036', N'4c98f160-d676-40a2-9b88-79fd1343f333', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
INSERT [dbo].[LayoutTypeProperty] ([Id], [LayoutTypeId], [PropertyId]) VALUES (N'fe7be0f0-5bc3-4da2-8254-e0221b8b919f', N'5a0a5884-da84-4922-a02f-5828b55d5c92', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
INSERT [dbo].[LayoutTypeProperty] ([Id], [LayoutTypeId], [PropertyId]) VALUES (N'21c85e09-737a-4596-8432-eab3797ebc91', N'9341f92e-83d8-4afe-ad4a-a95deeda9ae3', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
INSERT [dbo].[LayoutTypeProperty] ([Id], [LayoutTypeId], [PropertyId]) VALUES (N'47361d1e-dbdd-4b2f-8c0c-f03f2771c04d', N'43734210-943e-4f33-a161-f12260b8c001', N'f5031c31-778b-45dd-bd33-eeb2a088d2bc')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[Layout] ([Id], [Config], [IsDeleted], [Name]) VALUES (N'c4843750-1e7c-45f9-bf11-08d3a4535e73', N'[{"Id":"257d3d1f-9047-d834-837c-afa9c2a46539","Type":"container","LayoutTemplate":"container","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"cbffe4fd-e4ed-7568-73e6-1c3651fded07","Type":"row","LayoutTemplate":"row","SortOrder":2,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"7b5c0a20-ca06-49c1-b69b-47de5cde810f","Type":"column","LayoutTemplate":"column","SortOrder":2,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[]}]}]}]', 0, N'Contact Layout')
GO
INSERT [dbo].[Layout] ([Id], [Config], [IsDeleted], [Name]) VALUES (N'af7533aa-3e4b-4e29-b118-1841ab5a0a91', N'[{"Id":"afbc401a-385d-0bbf-0beb-b67b583070e6","Type":"container","LayoutTemplate":"container","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"e3971140-3b9c-b79b-695c-f8f081a8cbd2","Type":"row","LayoutTemplate":"row","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"cc9c62d1-f63b-3d6d-96c0-39dbfb1444f6","Type":"column","LayoutTemplate":"column","SortOrder":2,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[]}]}]}]', 0, N'Login Layout')
GO
INSERT [dbo].[Layout] ([Id], [Config], [IsDeleted], [Name]) VALUES (N'd6471f27-716c-4f6f-a10b-4acef3fa4da3', N'[{"Id":"f2de4bd6-c7a0-d4df-3377-7f4b2d4f3e45","Type":"wrapper","LayoutTemplate":"repeater","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[]},{"Id":"5d4dc519-c60f-1b5b-7a5e-7c95371fe6a5","Type":"container","LayoutTemplate":"container","SortOrder":2,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"a3554eba-a66e-95ea-c33f-4c23041c7ab5","Type":"row","LayoutTemplate":"row","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[{"Id":"ba37a7fe-c700-7def-7a20-737e7b1c118b","Type":"column","LayoutTemplate":"column","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[]}]}]}]', 0, N'Home Page Layout')
GO
INSERT [dbo].[Layout] ([Id], [Config], [IsDeleted], [Name]) VALUES (N'af8ecc7d-e300-41af-b55a-deeb097836d2', N'[{"Id":"ce36ceb1-7da5-504d-2201-a7314a91f161","Type":"wrapper","LayoutTemplate":"repeater","SortOrder":1,"Module":null,"Properties":[{"PropertyName":null,"PropertyValue":null}],"PlaceHolders":[]}]', 0, N'Admin Layout')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'654f660d-9c71-48f9-8237-593a39a0dbc0', NULL, N'Security Roles', 1, N'Security Roles', NULL, N'SecurityRoles', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'e4792855-5df8-4186-ad32-69d6464c748f', NULL, N'Login', 1, N'Login', NULL, N'Security', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'f07dbddf-4937-42b8-9bee-9c0713128013', NULL, N'File Management', 1, N'File Management', NULL, N'FileManager', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'0c30609d-87f3-4d84-9269-cfba91e5c0b6', NULL, N'User Management', 1, N'User Management', NULL, N'UserManagement', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'f32fa4c5-d319-48b0-a68b-cffb9c8743d5', NULL, N'Content Management', 1, N'Content Management', NULL, N'ContentManagement', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'57813091-da9f-47e3-9d63-dd5c4df79f1d', NULL, N'Page Management', 1, N'Page Management', NULL, N'PageManagement', N'00.01.00')
GO
INSERT [dbo].[Module] ([Id], [CreatedDate], [Description], [IsActive], [Label], [LastModifiedDate], [Name], [Version]) VALUES (N'73829a91-4a4a-4c22-885a-fb1215e37fdc', NULL, N'Language', 1, N'Language', NULL, N'Language', N'00.01.00')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[ModuleActionType] ([Id], [ControlType]) VALUES (N'72366792-3740-4e6b-b960-9c9c5334163a', N'View')
GO
INSERT [dbo].[ModuleActionType] ([Id], [ControlType]) VALUES (N'192278b6-7bf2-40c2-a776-b9ca5fb04fbb', N'Edit')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'1b56fde5-4494-4cf7-88db-2dc7b942284b', N'Index', N'Home', N'Deviser.Modules.FileManager.Controllers', N'File Manager', N'fa fa-files-o', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'f07dbddf-4937-42b8-9bee-9c0713128013')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'57415ac9-9141-495a-a25d-4a80f1c5827a', N'LayoutTypes', N'Home', N'Deviser.Modules.ContentManagement.Controllers', N'Layout Types', NULL, NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'f32fa4c5-d319-48b0-a68b-cffb9c8743d5')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'54df0a1f-99b0-4847-91f5-7cd187818fe3', N'Index', N'Home', N'Deviser.Modules.SecurityRoles.Controllers', N'Security Roles', N'fa fa-shield', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'654f660d-9c71-48f9-8237-593a39a0dbc0')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'7154eb95-36cc-488e-8d24-83b60f3ffffa', N'ContentTypes', N'Home', N'Deviser.Modules.ContentManagement.Controllers', N'Content Types', NULL, NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'f32fa4c5-d319-48b0-a68b-cffb9c8743d5')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'37ec5283-1fec-4779-bd43-9718c5648ffb', N'Index', N'Home', N'Deviser.Modules.UserManagement.Controllers', N'User Management', N'fa fa-users', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'0c30609d-87f3-4d84-9269-cfba91e5c0b6')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'83998364-707b-49ef-abed-b01f864bfe4a', N'Index', N'Home', N'Deviser.Modules.PageManagement.Controllers', N'Page Management', N'fa fa-file-o', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'57813091-da9f-47e3-9d63-dd5c4df79f1d')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'22d7f353-68c6-4c80-b261-c4d21b942623', N'Login', N'Account', N'Deviser.Modules.Security.Controllers', N'Login', N'fa fa-sign-in', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'e4792855-5df8-4186-ad32-69d6464c748f')
GO
INSERT [dbo].[ModuleAction] ([Id], [ActionName], [ControllerName], [ControllerNamespace], [DisplayName], [IconClass], [IconImage], [IsDefault], [ModuleActionTypeId], [ModuleId]) VALUES (N'5601b5eb-230f-4a43-a906-fed2923aca74', N'Index', N'Home', N'Deviser.Modules.Language.Controllers', N'Language', N'fa fa-language', NULL, 1, N'72366792-3740-4e6b-b960-9c9c5334163a', N'73829a91-4a4a-4c22-885a-fb1215e37fdc')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'd1c11d1f-2345-43b6-baa1-8ce890242397', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 1, 0, CAST(N'2016-07-07 23:57:54.633' AS DateTime), NULL, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'dd650840-0ee7-46cf-abb5-8a1591dc0936', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-03 19:36:03.907' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 1, 4, N'd1c11d1f-2345-43b6-baa1-8ce890242397', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'c597d915-38e0-4c32-0615-08d3a367fbcc', CAST(N'2016-07-03 19:32:15.350' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-04 22:57:28.963' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 2, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'56b72d88-5922-4635-0616-08d3a367fbcc', CAST(N'2016-07-03 19:34:47.250' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-04 22:58:39.287' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 3, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'8efd99d2-5004-44c6-0617-08d3a367fbcc', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-04 22:59:11.037' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 4, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'56ff05c4-57f6-429c-c4ad-08d3a6adbc78', CAST(N'2016-07-07 23:29:11.080' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-07 23:35:32.573' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 6, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'20d1b105-5c6d-4961-c4ae-08d3a6adbc78', CAST(N'2016-07-07 23:29:41.723' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-07 23:33:33.457' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 5, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'c6dd6902-4a9c-4a38-8a05-febe76694993', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 1, 1, CAST(N'2016-07-03 19:36:03.907' AS DateTime), N'af8ecc7d-e300-41af-b55a-deeb097836d2', 2, 1, N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'[G]Skins/Skyline/Admin.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'19391c5e-1253-40c3-3b4f-08d3a4490460', CAST(N'2016-07-04 22:47:02.990' AS DateTime), NULL, 0, 1, 0, CAST(N'2016-07-04 23:40:18.393' AS DateTime), N'c4843750-1e7c-45f9-bf11-08d3a4535e73', 1, 2, N'd1c11d1f-2345-43b6-baa1-8ce890242397', N'[G]Skins/Skyline/Home.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'd5d5a9fd-511b-4025-b495-8908fb70c762', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 1, 0, CAST(N'2016-07-04 23:46:51.437' AS DateTime), N'd6471f27-716c-4f6f-a10b-4acef3fa4da3', 1, 1, N'd1c11d1f-2345-43b6-baa1-8ce890242397', N'[G]Skins/Skyline/Home.cshtml', NULL)
GO
INSERT [dbo].[Page] ([Id], [CreatedDate], [EndDate], [IsDeleted], [IsIncludedInMenu], [IsSystem], [LastModifiedDate], [LayoutId], [PageLevel], [PageOrder], [ParentId], [SkinSrc], [StartDate]) VALUES (N'62328d72-ad82-4de2-9a98-c954e5b30b28', CAST(N'2016-07-03 19:34:55.593' AS DateTime), NULL, 0, 0, 0, CAST(N'2016-07-04 22:18:02.533' AS DateTime), N'af7533aa-3e4b-4e29-b118-1841ab5a0a91', 1, 3, N'd1c11d1f-2345-43b6-baa1-8ce890242397', N'[G]Skins/Skyline/Home.cshtml', NULL)
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'378d40e1-403d-5c58-01bc-468fe5bbb9ab', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'54df0a1f-99b0-4847-91f5-7cd187818fe3', N'654f660d-9c71-48f9-8237-593a39a0dbc0', N'56b72d88-5922-4635-0616-08d3a367fbcc', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'c1b960f5-7073-ea28-ac32-4ec59bf9e890', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'5601b5eb-230f-4a43-a906-fed2923aca74', N'73829a91-4a4a-4c22-885a-fb1215e37fdc', N'8efd99d2-5004-44c6-0617-08d3a367fbcc', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'd4f7f41d-d5ef-a619-3e0f-569a0a53ae02', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'57415ac9-9141-495a-a25d-4a80f1c5827a', N'f32fa4c5-d319-48b0-a68b-cffb9c8743d5', N'20d1b105-5c6d-4961-c4ae-08d3a6adbc78', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'35349328-976f-4dd0-bf74-57bc523caab8', N'afbc401a-385d-0bbf-0beb-b67b583070e6', 0, N'22d7f353-68c6-4c80-b261-c4d21b942623', N'e4792855-5df8-4186-ad32-69d6464c748f', N'62328d72-ad82-4de2-9a98-c954e5b30b28', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'ddeddbf7-3e60-88cf-60c0-9bfb56c3ad89', N'f2de4bd6-c7a0-d4df-3377-7f4b2d4f3e45', 1, N'1b56fde5-4494-4cf7-88db-2dc7b942284b', N'f07dbddf-4937-42b8-9bee-9c0713128013', N'd5d5a9fd-511b-4025-b495-8908fb70c762', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'f9d2f198-e489-6533-00f7-c25d8d920fee', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'37ec5283-1fec-4779-bd43-9718c5648ffb', N'0c30609d-87f3-4d84-9269-cfba91e5c0b6', N'c597d915-38e0-4c32-0615-08d3a367fbcc', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'beb22251-396f-4e69-91ab-c28aa93f7bde', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'83998364-707b-49ef-abed-b01f864bfe4a', N'57813091-da9f-47e3-9d63-dd5c4df79f1d', N'c6dd6902-4a9c-4a38-8a05-febe76694993', 1)
GO
INSERT [dbo].[PageModule] ([Id], [ContainerId], [IsDeleted], [ModuleActionId], [ModuleId], [PageId], [SortOrder]) VALUES (N'720785c4-b947-ee8e-a835-ea1fa95b1c30', N'ce36ceb1-7da5-504d-2201-a7314a91f161', 0, N'7154eb95-36cc-488e-8d24-83b60f3ffffa', N'f32fa4c5-d319-48b0-a68b-cffb9c8743d5', N'56ff05c4-57f6-429c-c4ad-08d3a6adbc78', 1)
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[PageContent] ([Id], [ContainerId], [ContentTypeId], [CreatedDate], [IsDeleted], [LastModifiedDate], [PageId], [Properties], [SortOrder]) VALUES (N'd9ef8bc1-6706-fe74-b3e0-7f54dfcc099b', N'ba37a7fe-c700-7def-7a20-737e7b1c118b', N'00332002-f2c7-401c-b59c-d0181eaf657b', NULL, 0, CAST(N'2016-07-04 23:46:49.317' AS DateTime), N'd5d5a9fd-511b-4025-b495-8908fb70c762', N'[{"id":"f5031c31-778b-45dd-bd33-eeb2a088d2bc","name":"cssClass","label":"Css Class","value":null,"propertyOptionListId":null,"propertyOptionList":null}]', 1)
GO
INSERT [dbo].[PageContent] ([Id], [ContainerId], [ContentTypeId], [CreatedDate], [IsDeleted], [LastModifiedDate], [PageId], [Properties], [SortOrder]) VALUES (N'4b2166a4-1c9c-7368-c5ba-e0799ef6f1ed', N'f2de4bd6-c7a0-d4df-3377-7f4b2d4f3e45', N'd2e62921-32f5-4c66-a9b3-e5b61d60b193', NULL, 0, CAST(N'2016-07-04 23:46:51.360' AS DateTime), N'd5d5a9fd-511b-4025-b495-8908fb70c762', N'[{"id":"f5031c31-778b-45dd-bd33-eeb2a088d2bc","name":"cssClass","label":"Css Class","value":null,"propertyOptionListId":null,"propertyOptionList":null}]', 1)
GO
-----------------------------------------------------------------------------------
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'c597d915-38e0-4c32-0615-08d3a367fbcc', N'en-US', NULL, NULL, N'User Management', N'User Management', N'en-us/Admin/UserManagement')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'56b72d88-5922-4635-0616-08d3a367fbcc', N'en-US', N'Security Roles', NULL, N'Security Roles', N'Security Roles', N'en-us/Admin/SecurityRoles')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'8efd99d2-5004-44c6-0617-08d3a367fbcc', N'en-US', NULL, NULL, N'Languages', N'Languages', N'en-us/Admin/Languages')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'19391c5e-1253-40c3-3b4f-08d3a4490460', N'en-US', NULL, NULL, N'Contact', N'Contact', N'en-us/Contact')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'56ff05c4-57f6-429c-c4ad-08d3a6adbc78', N'en-US', N'Content Types', NULL, N'Content Types', N'Content Types', N'en-us/Admin/ContentTypes')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'20d1b105-5c6d-4961-c4ae-08d3a6adbc78', N'en-US', N'Layout Types', NULL, N'Layout Types', N'Layout Types', N'en-us/Admin/LayoutTypes')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'd5d5a9fd-511b-4025-b495-8908fb70c762', N'de-CH', NULL, NULL, N'Homedech', N'Home Page', N'de-ch/Homedech')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'd5d5a9fd-511b-4025-b495-8908fb70c762', N'en-US', NULL, NULL, N'Home', N'Home Page', N'en-us/Home')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'd5d5a9fd-511b-4025-b495-8908fb70c762', N'fr-CH', NULL, NULL, N'Homefrch', N'Home Page', N'fr-ch/Homefrch')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'dd650840-0ee7-46cf-abb5-8a1591dc0936', N'en-US', NULL, NULL, N'Admin', N'Admin', N'en-us/Admin')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'62328d72-ad82-4de2-9a98-c954e5b30b28', N'en-US', NULL, NULL, N'Login', N'Login', N'en-us/Login')
GO
INSERT [dbo].[PageTranslation] ([PageId], [Locale], [Description], [Keywords], [Name], [Title], [URL]) VALUES (N'c6dd6902-4a9c-4a38-8a05-febe76694993', N'en-US', NULL, NULL, N'Page Management', N'Page Management', N'en-us/Admin/PageManagement')
GO
-----------------------------------------------------------------------------------
INSERT [dbo].[PageContentTranslation] ([Id], [ContentData], [CreatedDate], [CultureCode], [IsDeleted], [LastModifiedDate], [PageContentId]) VALUES (N'2d909cd8-aeca-431e-c11f-08d3a4546237', N'Test content', CAST(N'2016-07-04 23:44:32.133' AS DateTime), N'en-US', 0, CAST(N'2016-07-04 23:44:32.133' AS DateTime), N'd9ef8bc1-6706-fe74-b3e0-7f54dfcc099b')
GO
INSERT [dbo].[PageContentTranslation] ([Id], [ContentData], [CreatedDate], [CultureCode], [IsDeleted], [LastModifiedDate], [PageContentId]) VALUES (N'2a18d1c7-4377-4c22-c120-08d3a4546237', N'test content de', CAST(N'2016-07-04 23:45:08.567' AS DateTime), N'de-CH', 0, CAST(N'2016-07-04 23:45:08.567' AS DateTime), N'd9ef8bc1-6706-fe74-b3e0-7f54dfcc099b')
GO
INSERT [dbo].[PageContentTranslation] ([Id], [ContentData], [CreatedDate], [CultureCode], [IsDeleted], [LastModifiedDate], [PageContentId]) VALUES (N'c2fd183e-f59a-4d99-c121-08d3a4546237', N'test content fr', CAST(N'2016-07-04 23:45:19.117' AS DateTime), N'fr-CH', 0, CAST(N'2016-07-04 23:45:19.117' AS DateTime), N'd9ef8bc1-6706-fe74-b3e0-7f54dfcc099b')
GO
INSERT [dbo].[PageContentTranslation] ([Id], [ContentData], [CreatedDate], [CultureCode], [IsDeleted], [LastModifiedDate], [PageContentId]) VALUES (N'a9c805e9-7e43-4830-c122-08d3a4546237', N'{"items":[{"title":"Test title","description":"Lorem ipsum dolor sit amet, eu mei vero ubique iisque, ne affert aperiri meliore eum. Cu cibo choro indoctum vim, vel eros aperiri te.","imageUrl":"/assets/images/Flea-Chase.jpg?92.45161651695493","id":"3e6073e1-1021-06bf-016e-6e3adaefb504","viewOrder":1},{"title":"Test title1","imageUrl":"/assets/images/images (1).jpg?77.31950553761884","description":"Possit voluptua eos et, tibique contentiones duo ei, suavitate deseruisse ne vix. Eos ne facer persius.","id":"08b8b64d-dfdc-c9cc-f41e-f0424dd0bab6","viewOrder":2},{"imageUrl":"/assets/images/images.jpg?69.52002824251373","description":"Ad nec discere eruditi volutpat, qui quem eligendi te, indoctum facilisis omittantur ex ius.","title":"Title 3","id":"f0f0e9a4-076e-4d47-9ed7-0a18d5a51b37","viewOrder":3}]}', CAST(N'2016-07-04 23:48:08.860' AS DateTime), N'en-US', 0, CAST(N'2016-07-04 23:48:13.770' AS DateTime), N'4b2166a4-1c9c-7368-c5ba-e0799ef6f1ed')
GO
-----------------------------------------------------------------------------------