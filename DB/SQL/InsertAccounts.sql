ALTER Procedure [dbo].[InsertAccounts](@Id bigint,
										@Name NVarChar(30),
										@AccountTypeId bigint,
										@InterestRate real, 
										@MonthsToKeep int, 
										@IsActive bit)
AS

Insert into dbo.Accounts
(AccountId, AccountTypeId, InterestRate, MonthsToKeepTrans, IsActive, AccountName)
values
(@Id, @AccountTypeId, @InterestRate, @MonthsToKeep, @IsActive, @Name)

GO


