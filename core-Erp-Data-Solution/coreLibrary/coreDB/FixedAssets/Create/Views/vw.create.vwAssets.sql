use coreDB
go

alter view vwAssets 
with encryption as
SELECT   distinct     fa.asset.assetTag, fa.asset.assetDescription, fa.asset.assetPurchaseDate, fa.asset.assetLifetime, fa.asset.assetPrice, fa.asset.assetCurrentValue, 
                         fa.asset.depreciationRate, fa.assetSubCategory.assetSubCategoryName, fa.assetCategory.assetCategoryName, 
						 dbo.vw_ou.ou_name, fa.staff.staffNo, isnull(fa.assetCategory.companyId,0) as companyId, isnull(comp_Name, '') AS comp_Name, 
                         isnull(fa.staff.surName + ',' + fa.staff.otherNames,'') AS staffName,
						 isnull(fa.asset.staffid,0) staffid, isnull(fa.asset.ouID,0) ouID, 
						 fa.assetSubCategory.assetCategoryID, fa.asset.assetSubCategoryID, fa.asset.assetID
FROM            fa.asset INNER JOIN
                         fa.assetSubCategory ON fa.asset.assetSubCategoryID = fa.assetSubCategory.assetSubCategoryID INNER JOIN
                         fa.assetCategory ON fa.assetSubCategory.assetCategoryID = fa.assetCategory.assetCategoryID LEFT OUTER JOIN
                         fa.staff ON fa.asset.staffID = fa.staff.staffID LEFT OUTER JOIN
                         dbo.vw_ou ON fa.asset.ouID = dbo.vw_ou.ou_id
						 left join dbo.comp_prof AS company on fa.assetCategory.companyId = company.companyId

go

alter view vwAssetDepreciation
with encryption as
SELECT        fa.asset.assetTag, fa.asset.assetDescription, fa.asset.assetPurchaseDate, fa.asset.assetLifetime, fa.asset.assetPrice, fa.asset.assetCurrentValue, 
                         fa.asset.depreciationRate, fa.assetSubCategory.assetSubCategoryName, fa.assetCategory.assetCategoryName, dbo.vw_ou.ou_name, fa.staff.staffNo, 
                         fa.staff.surName + ',' + fa.staff.otherNames AS staffName, 
						 case when ISNULL(SUM(fa.assetDepreciation.depreciationAmount),0)=0 then
						  isnull(sum(j.amt),0)
						  else ISNULL(SUM(fa.assetDepreciation.depreciationAmount),0) end
						   AS depreciationAmount,
						 isnull(fa.asset.staffid,0) staffid, isnull(fa.asset.ouID,0) ouID, 
						 fa.assetSubCategory.assetCategoryID, fa.asset.assetSubCategoryID, fa.asset.assetID,
						 isnull(fa.assetCategory.companyId,0) as companyId, isnull(comp_Name, '') AS comp_Name
FROM            fa.asset INNER JOIN
                         fa.assetSubCategory ON fa.asset.assetSubCategoryID = fa.assetSubCategory.assetSubCategoryID INNER JOIN
                         fa.assetCategory ON fa.assetSubCategory.assetCategoryID = fa.assetCategory.assetCategoryID LEFT OUTER JOIN
                         fa.assetDepreciation ON fa.asset.assetID = fa.assetDepreciation.assetID LEFT OUTER JOIN
                         fa.staff ON fa.asset.staffID = fa.staff.staffID AND fa.asset.staffID = fa.staff.staffID LEFT OUTER JOIN
                         dbo.vw_ou ON fa.asset.ouID = dbo.vw_ou.ou_id
						 left outer join
						 (
							select
								a.assetID,
								sum(dbt_amt) as amt
							from fa.asset a inner join dbo.jnl j on a.assettag=j.ref_no 
							group by a.assetID
						 ) j on fa.asset.assetID = j.assetID
						  left join dbo.comp_prof AS company on fa.assetCategory.companyId = company.companyId

GROUP BY fa.asset.assetTag, fa.asset.assetDescription, fa.asset.assetPurchaseDate, fa.asset.assetLifetime, fa.asset.assetPrice, fa.asset.assetCurrentValue, 
                         fa.asset.depreciationRate, fa.assetSubCategory.assetSubCategoryName, fa.assetCategory.assetCategoryName, dbo.vw_ou.ou_name, fa.staff.staffNo, 
                         fa.staff.surName + ',' + fa.staff.otherNames,
						 isnull(fa.asset.staffid,0) , isnull(fa.asset.ouID,0) , 
						 fa.assetSubCategory.assetCategoryID, fa.asset.assetSubCategoryID, fa.asset.assetID,
						  isnull(fa.assetCategory.companyId,0) , isnull(comp_Name, '') 

go

alter procedure getAssetDepreciation
(
	@startDate datetime,
	@endDate datetime
)
with encryption as
declare @sd datetime, @ed datetime
	select @sd=
		cast(''+cast(datepart(yyyy,@startDate) as nvarchar(4))+'-'+cast(datepart(mm,@startDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@startDate) as nvarchar(2)) as datetime)
	select @ed=
		cast(''+cast(datepart(yyyy,@endDate) as nvarchar(4))+'-'+cast(datepart(mm,@endDate) as nvarchar(2))
		+'-'+cast(datepart(dd,@endDate) as nvarchar(2)) + ' 23:59:59' as datetime)
SELECT        fa.asset.assetTag, fa.asset.assetDescription, fa.asset.assetPurchaseDate, fa.asset.assetLifetime, fa.asset.assetPrice, fa.asset.assetCurrentValue, 
                         fa.asset.depreciationRate, fa.assetSubCategory.assetSubCategoryName, fa.assetCategory.assetCategoryName, dbo.vw_ou.ou_name, fa.staff.staffNo, 
                         fa.staff.surName + ',' + fa.staff.otherNames AS staffName,
						 case when ISNULL(SUM(fa.assetDepreciation.depreciationAmount),0)=0 then
						  isnull(sum(j.amt),0)
						  else ISNULL(SUM(fa.assetDepreciation.depreciationAmount),0) end
						   AS depreciationAmount,
						 isnull(fa.asset.staffid,0) staffid, isnull(fa.asset.ouID,0) ouID, 
						 fa.assetSubCategory.assetCategoryID, fa.asset.assetSubCategoryID, fa.asset.assetID
FROM            fa.asset INNER JOIN
                         fa.assetSubCategory ON fa.asset.assetSubCategoryID = fa.assetSubCategory.assetSubCategoryID INNER JOIN
                         fa.assetCategory ON fa.assetSubCategory.assetCategoryID = fa.assetCategory.assetCategoryID LEFT OUTER JOIN
                         fa.assetDepreciation ON fa.asset.assetID = fa.assetDepreciation.assetID LEFT OUTER JOIN
                         fa.staff ON fa.asset.staffID = fa.staff.staffID AND fa.asset.staffID = fa.staff.staffID LEFT OUTER JOIN
                         dbo.vw_ou ON fa.asset.ouID = dbo.vw_ou.ou_id
						 left outer join
						 (
							select
								a.assetID,
								sum(dbt_amt) as amt
							from fa.asset a inner join dbo.jnl j on a.assettag=j.ref_no
							where tx_date <= @ed
							group by a.assetID
						 ) j on fa.asset.assetID = j.assetID
where (fa.assetDepreciation.drepciationDate is null or 
	fa.assetDepreciation.drepciationDate between @sd and @ed)
GROUP BY fa.asset.assetTag, fa.asset.assetDescription, fa.asset.assetPurchaseDate, fa.asset.assetLifetime, fa.asset.assetPrice, fa.asset.assetCurrentValue, 
                         fa.asset.depreciationRate, fa.assetSubCategory.assetSubCategoryName, fa.assetCategory.assetCategoryName, dbo.vw_ou.ou_name, fa.staff.staffNo, 
                         fa.staff.surName + ',' + fa.staff.otherNames,
						 isnull(fa.asset.staffid,0) , isnull(fa.asset.ouID,0) , 
						 fa.assetSubCategory.assetCategoryID, fa.asset.assetSubCategoryID, fa.asset.assetID
go