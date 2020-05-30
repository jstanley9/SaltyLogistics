CREATE procedure [dbo].[GetConfig] (@section nvarchar(30),
                            @preference nvarchar(30),
							@value nvarchar(1000) output)
as

Select @value = c.Value
From   dbo.Configs as c
where  c.Section = @section
  and  c.Preference = @preference

return 
GO


