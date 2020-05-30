ALTER procedure [dbo].[UpdateConfig] (@section nvarchar(30),
                               @preference nvarchar(30),
							   @value nvarchar(1000))
as

update dbo.Configs
   set Value = @value
 where section = @section
   and Preference = @preference

if @@ROWCOUNT = 0 
	insert into dbo.Configs
	(Section, Preference, Value)
	values (@section, @preference, @value)

return
GO


