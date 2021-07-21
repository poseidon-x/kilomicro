use coreDB
go

CREATE View vw_cities 
with encryption AS

SELECT     TOP (100) PERCENT country_name, region_name, city_name, district_name,
		i.city_id, c.country_id, r.region_id, d.district_id
FROM         dbo.countries AS c INNER JOIN
				dbo.regions r on c.country_id=r.country_id INNER JOIN
				dbo.districts d on r.region_id=d.region_id  INNER JOIN
                dbo.cities AS i ON d.district_id = i.district_id 
ORDER BY country_name, region_name, district_name, city_name


GO
 