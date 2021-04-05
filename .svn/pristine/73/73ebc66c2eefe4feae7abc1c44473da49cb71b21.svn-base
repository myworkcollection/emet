update TQuoteDetails set NetProfDisc = (CONVERT(nvarchar,
							ROUND(
							convert(float,
							(
							case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
							else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
							/convert(float,ISNULL(FinalQuotePrice,0))
							)
							*100)
							,1)
							))
where VendorCode1 not in (select VendorCode from MDM.dbo.TSBMPRICINGPOLICY)