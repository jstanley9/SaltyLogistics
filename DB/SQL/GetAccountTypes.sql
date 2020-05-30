ALTER Procedure [dbo].[GetAccountTypesActive]
as

Select at.AccountTypeId, at.IsActive, at.IsAsset, at.IsInterestComputed, at.AccountTypeName as Name
From   dbo.AccountTypes as at
where  at.IsActive = 1
order by at.AccountTypeName

return 
GO


