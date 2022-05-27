CREATE TABLE [dbo].[tblLoanApp1Geolocation](
	ID int IDENTITY(1,1) NOT NULL,
	LoanAppID  nvarchar(max),
	ParentID nvarchar(max) NULL,
	CID nvarchar(max) NULL,
	CustName nvarchar(max) NULL,
	BranchName nvarchar(max) NULL,
	AMName nvarchar(max) NULL,
	COName nvarchar(max) NULL,
	VBName nvarchar(max) NULL,
	LoanAmount nvarchar(max) NULL,
	LoanCurrency nvarchar(max) NULL,
	LoanProduct nvarchar(max) NULL,
	AMKDL nvarchar(max) NULL,
	LoanAA nvarchar(max) NULL,
	DateIn DateTime,
	LoanCreateGeoLocation nvarchar(max),
    LoanSubmitGeoLocation nvarchar(max),
    StartDate DateTime,
    EndDate DateTime,
    CashFlowStartDate DateTime,
    CashFlowEndDate DateTime,
    CashFlowStartGeoLocation nvarchar(max) NULL,
    CashFLowEndGeoLocation nvarchar(max) NULL,
    LoanStatus nvarchar(25) 
CONSTRAINT PK_tblLoanApp1Geolocation PRIMARY KEY CLUSTERED 
(
 ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[tblLoanAppDisbuseGeolocation](
	ID int IDENTITY(1,1) NOT NULL,
	CID nvarchar(max) NULL,
	CustName nvarchar(max) NULL,
	BranchName nvarchar(max) NULL,
	AMName nvarchar(max) NULL,
	COName nvarchar(max) NULL,
	VBName nvarchar(max) NULL,
	DisbAmount nvarchar(max) NULL,
	LoanCurrency nvarchar(max) NULL,
	LoanProduct nvarchar(max) NULL,
	AMKDL nvarchar(max) NULL,
	LoanAA nvarchar(max) NULL,
	DateIn DateTime,
	Geolocation nvarchar(max) NULL,
	StartDate DateTime,
    EndDate DateTime
CONSTRAINT PK_tblLoanAppDisbuseGeolocation PRIMARY KEY CLUSTERED 
(
 ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblLoanAppRepayGeolocation](
	ID int IDENTITY(1,1) NOT NULL,
	CID nvarchar(max) NULL,
	CustName nvarchar(max) NULL,
	BranchName nvarchar(max) NULL,
	AMName nvarchar(max) NULL,
	COName nvarchar(max) NULL,
	VBName nvarchar(max) NULL,
	CollAmount nvarchar(max),
	LoanCurrency nvarchar(max) NULL,
	LoanProduct nvarchar(max) NULL,
	AMKDL nvarchar(max) NULL,
	LoanAA nvarchar(max) NULL,
	DateIn DateTime,
	Geolocation nvarchar(max),
	StartDate DateTime,
    EndDate DateTime
CONSTRAINT PK_tblLoanAppRepayGeolocation PRIMARY KEY CLUSTERED 
(
 ID ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE PROC [dbo].[AddGeolocationLoanAppCreation]
	 @LoanAppID nvarchar(125)
	,@LoanAmount nvarchar(225)
	,@LoanCurrency nvarchar(225)
	,@LoanProduct nvarchar(225)
	,@LoanAA nvarchar(225) = NULL
	,@LoanCreateGeoLocation nvarchar(225)
    ,@LoanSubmitGeoLocation nvarchar(225)
    ,@StartDate  nvarchar(125)
    ,@EndDate  nvarchar(125)
    ,@CashFlowStartDate  nvarchar(125)
    ,@CashFlowEndDate  nvarchar(125)
    ,@CashFlowStartGeoLocation nvarchar(225)
    ,@CashFLowEndGeoLocation nvarchar(225)
    ,@LoanStatus nvarchar(25) 
AS
BEGIN
	DECLARE	@ParentID nvarchar(225)= NULL
		,@CID nvarchar(225)
		,@CustName nvarchar(225)
		,@BranchName nvarchar(225)
		,@AMName nvarchar(225)
		,@COName nvarchar(225)
		,@VBName nvarchar(225)
	SELECT 
		@ParentID = l.ParentID
		,@BranchName= tf.OfficeID +''+tf.OfficeName
		,@CID = p.CustomerID
		,@CustName = p.LastName+' '+ p.FirstName
		,@COName =th.ID+' '+ th.Name
		,@AMName =thh.ID+'-'+thh.Name
		,@VBName=vb.VBID +'-'+vb.VBName
	FROM tblLoanApp1 l
		LEFT JOIN tblLoanAppPerson2 p ON p.LoanAppID = l.LoanAppID
		LEFT JOIN tblUser u ON u.UserID = l.CreateBy
		LEFT JOIN T24_Office tf ON tf.OfficeID = u.OfficeID
		LEFT JOIN T24_OfficeHierachy th ON th.ID=u.OfficeHierachyID 
		LEFT JOIN T24_OfficeHierachy thh ON thh.ID=th.ParentID
		LEFT JOIN T24_VBLink vb ON vb.VBID = p.VillageBankID
	WHERE 
			l.LoanAppID=@LoanAppID
			AND p.LoanAppPersonTypeID='31'
	
	INSERT INTO tblLoanApp1Geolocation(LoanAppID,ParentID,CID,CustName,BranchName,AMName,COName,VBName,LoanAmount,LoanCurrency
		,LoanProduct,AMKDL,LoanAA,DateIn,LoanCreateGeoLocation,LoanSubmitGeoLocation,StartDate,EndDate,CashFlowStartDate,CashFlowEndDate
		,CashFlowStartGeoLocation,CashFLowEndGeoLocation,LoanStatus)
	VALUES(@LoanAppID,@ParentID,@CID,@CustName,@BranchName,@AMName,@COName,@VBName,@LoanAmount,@LoanCurrency
		,@LoanProduct,@AMKDL,@LoanAA,GETDATE(),@LoanCreateGeoLocation,@LoanSubmitGeoLocation,@StartDate,@EndDate,@CashFlowStartDate,@CashFlowEndDate
		,@CashFlowStartGeoLocation,@CashFLowEndGeoLocation,@LoanStatus)
END
GO
CREATE  PROC [dbo].[AddGeolocationLoanAppDisbuse]
	 @CID nvarchar(225)
	,@CustName nvarchar(225)
	,@DisbAmount nvarchar(225)
	,@LoanCurrency nvarchar(225)
	,@LoanProduct nvarchar(225)
	,@LoanAA nvarchar(225)
    ,@Geolocation nvarchar(225)
    ,@StartDate  nvarchar(125)
    ,@EndDate  nvarchar(125) 
AS
BEGIN
	DECLARE @BranchName nvarchar(225)
	,@AMName nvarchar(225)
	,@COName nvarchar(225)
	,@VBName nvarchar(225)

	SELECT
	 @BranchName= f.OfficeID +''+f.OfficeName
	,@COName =oh.ID+' '+ oh.Name
	,@VBName=r.VBID +'-'+r.VBRefer
	,@AMName =thh.ID+'-'+thh.Name
	FROM
	T24_DisbPost p
	LEFT JOIN T24_Disb r ON r.DisbID=p.DisbID
	LEFT JOIN T24_Office f ON f.OfficeID = r.BranchCode
	LEFT JOIN T24_OfficeHierachy oh ON oh.ID=r.COID
	LEFT JOIN T24_OfficeHierachy thh ON thh.ID=oh.ParentID
	WHERE
	r.LoanRef=@LoanAA

	INSERT INTO tblLoanAppDisbuseGeolocation(CID,CustName,BranchName,AMName,COName,VBName,DisbAmount,LoanCurrency,
	LoanProduct,AMKDL,LoanAA,DateIn,Geolocation,StartDate,EndDate)
	VALUES(@CID,@CustName,@BranchName,@AMName,@COName,@VBName,@DisbAmount,@LoanCurrency
	,@LoanProduct,@AMKDL,@LoanAA,GETDATE(),@Geolocation,@StartDate,@EndDate)
END
GO
CREATE PROC [dbo].[AddGeolocationLoanAppRepay]
	@CID nvarchar(225) 
	,@CustName nvarchar(225)
	,@CollAmount nvarchar(225)
	,@LoanCurrency nvarchar(225)
	,@LoanProduct nvarchar(225)
	,@RepayID nvarchar(225)
    ,@Geolocation nvarchar(225)
    ,@StartDate  nvarchar(125)
    ,@EndDate  nvarchar(125) 
AS
BEGIN
	DECLARE @BranchName nvarchar(225)
	,@AMName nvarchar(225)
	,@COName nvarchar(225)
	,@VBName nvarchar(225)
	,@LoanAA nvarchar(225)
	SELECT
	 @BranchName= f.OfficeID +''+f.OfficeName
	,@COName =oh.ID+' '+ oh.Name
	,@VBName=vb.VBID +'-'+vb.VBName
	,@AMName =thh.ID+'-'+thh.Name
	,@LoanAA = r.LoanRef
	FROM
	T24_RepayPost p
	left join T24_Repay r ON r.RepayID=p.RepayID
	LEFT JOIN T24_Office f ON f.OfficeID = r.LoanBrnCode
	LEFT JOIN T24_OfficeHierachy oh ON oh.ID=r.COID
	LEFT JOIN T24_OfficeHierachy thh ON thh.ID=oh.ParentID
	LEFT JOIN T24_VBLink vb ON vb.VBID = r.VBRef
	WHERE
	r.RepayID = @RepayID

	INSERT INTO tblLoanAppRepayGeolocation(CID,CustName,BranchName,AMName,COName,VBName,CollAmount,LoanCurrency,LoanProduct,AMKDL,LoanAA
	,DateIn,Geolocation,StartDate,EndDate)
	VALUES(@CID,@CustName,@BranchName,@AMName,@COName,@VBName,@CollAmount,@LoanCurrency,@LoanProduct,@AMKDL,@LoanAA
	,GETDATE(),@Geolocation,@StartDate,@EndDate)
END

CREATE PROC AddLoanAppPersonAML
@LoanAppID nvarchar(125) 
,@LoanAppPersonID nvarchar(125) NULL
,@BlockStatus nvarchar(125) NULL
,@WatchListScreeningStatus nvarchar(125) NULL
,@WatchListCaseUrl nvarchar(125) NULL
,@RiskProfiling nvarchar(125) NULL
,@AMLApprovalStatus nvarchar(125) NULL
,@WatchListExposition nvarchar(125) NULL
,@ProductAndService nvarchar(125) NULL
,@CreateBy nvarchar(125) NULL
AS
BEGIN
	INSERT INTO tblLoanAppPersonAML(LoanAppID,LoanAppPersonID,BlockStatus,WatchListScreeningStatus
	,WatchListCaseUrl,RiskProfiling,AMLApprovalStatus,WatchListExposition,ProductAndService,CreateBy,CreateDate)
	VALUES(@LoanAppID,@LoanAppPersonID,@BlockStatus,@WatchListScreeningStatus,@WatchListCaseUrl
	,@RiskProfiling,@AMLApprovalStatus,@WatchListExposition,@ProductAndService,@CreateBy,GETDATE())
END