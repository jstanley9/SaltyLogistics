CREATE Procedure [dbo].[UpdateAccountActiveStatus] (@Id bigint,
													@isActive bit) 
AS

Update dbo.Accounts Set  IsActive = @IsActive
Where AccountId = @Id

GO
