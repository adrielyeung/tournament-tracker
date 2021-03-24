-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Adriel Yeung
-- Create date: 26/2/2021
-- Description:	Example of a stored procedure to select data
-- =============================================
CREATE PROCEDURE [dbo].[spPrizes_GetByTournament] 
	-- Add the parameters for the stored procedure here
	@TournamentId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT p.* FROM dbo.Prizes p
	INNER JOIN dbo.TournamentPrizes t ON p.id = t.PrizeId
	WHERE t.TournamentId = @TournamentId;
END
GO
