ALTER Procedure [dbo].[UpdateAccounts](@Id bigint,
									   @Name NvarChar(30),
									   @AccountTypeId bigint,
									   @InterestRate real, 
									   @MonthsToKeep int, 
									   @IsActive bit)
AS

Update dbo.Accounts Set  AccountName = @Name,
                         AccountTypeId = @AccountTypeId,
						 InterestRate = @InterestRate,
						 MonthsToKeepTrans = @MonthsToKeep,
						 IsActive = @IsActive
Where AccountId = @Id

GO


