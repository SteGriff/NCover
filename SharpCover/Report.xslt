<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" doctype-public="-W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/1999/REC-html401-19991224/loose.dtd" />
	<xsl:template match="/Report">
		<html>
			<head>
				<title>SharpCover coverage report - <xsl:value-of select="ReportName"/></title>
        <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
				<link rel="stylesheet" type="text/css" href="SharpCover.css" />
			</head>
			<body>
				<div id="container">
					<div id="header">
            <span id="title"/>
					</div>
					
					<div id="summary">
						<h3>SharpCover Coverage Report - <xsl:value-of select="ReportName"/>: <xsl:call-template name="percentage"/></h3>
						<span id="totalHit"><xsl:value-of select="NumberOfHitPoints"/> out of <xsl:value-of select="NumberOfPoints"/> coverage points hit.</span>
					</div>
					
					<div id="namespaceSummary">
						<h3>Namespace Coverage:</h3>
						<table>
							<xsl:apply-templates 
								select="Namespaces/*" 
								mode="summary"/>
						</table>
					</div>
					
					<div id="fileSummary">
						<h3>File Coverage:</h3>
						<xsl:apply-templates select="Namespaces/*"/>
					</div>	
						
					<xsl:if test="History">			
						<div id="history" style="height: 315px;">
							<div class="yAxisLabel100" style="display: inline; top: 0; float:left;">100%</div>
							<div class="yAxisLabel0" style="position: relative; display: inline; top: 295px; left:-20px; float:left;">0%</div>
							<xsl:for-each select="History/Events/Event">
								<xsl:sort select="position()" data-type="number" order="descending" />
							
								<span class="bar" style="width: 5px; position: absolute; left:{(position() * 8) + 32}px; background-color:{CoveragePercentageColor}; height:{round(CoveragePercentage * 100) * 3}px; top:{302 - (round(CoveragePercentage * 100) * 3)}px;">
                  <xsl:text>&#160;</xsl:text>
                </span>
							</xsl:for-each><br />
							<span class="xAxisLabel" style="position: relative; top: 285px; left: 40px; display: block; clear: both;">time &gt;</span>
						</div>
					</xsl:if>		
					
					<div id="footer">
						<p>Report generated on <xsl:value-of select="ReportDate"/> by <a href="http://sourceforge.net/projects/ncover/">SharpCover</a> version <xsl:value-of select="SharpCoverVersion"/></p>
						<p>Disclaimer: 100% Test coverage is no guarantee of the quality of the tests.</p>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>
	
	<xsl:template match="Namespace" mode="summary">
		<tr>
			<td><span class="namespace"><a href="#{generate-id(Name)}"><xsl:value-of select="Name"/></a></span></td>
			<td> - </td>
			<td><xsl:call-template name="percentage"/></td>
		</tr>
	</xsl:template>
	
	<xsl:template match="Namespace">
		<div class="namespaceFiles">
			<span class="namespace"><a name="{generate-id(Name)}"></a><xsl:value-of select="Name"/></span> -  
			<xsl:call-template name="percentage"/>
			<table>
				<thead>
					<tr>
						<th>Coverage</th>
						<th>File</th>
						<th>Missed Lines</th>
					</tr>
				</thead>
				<xsl:apply-templates select="Files/*"/>
			</table>
		</div>	
	</xsl:template>
	
	<xsl:template match="ReportFile">
		<tr>
			<td><xsl:call-template name="percentage"/> (<xsl:value-of select="NumberOfHitPoints"/>/<xsl:value-of select="NumberOfPoints"/>)</td>
			<td><a href="File:///{Filename}" title="Source file: {Name}"><xsl:value-of select="Name" /></a></td>
			<td><xsl:if test="string-length(MissedLineNumbers) > 0">[<xsl:value-of select="MissedLineNumbers"/>]</xsl:if></td>		
		</tr>
	</xsl:template>
	
	<xsl:template name="percentage">
		<span style="color:{CoveragePercentageColor};">
			<xsl:value-of select="format-number(number(NumberOfHitPoints) div number(NumberOfPoints), '00.##%')"/>	
		</span>
	</xsl:template>
</xsl:stylesheet>  