<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ms="urn:schemas-microsoft-com:xslt"
                xmlns:v="urn:schemas-microsoft-com:vml"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                xmlns:svg="http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd" >
  <xsl:param name = "CurrNodeID"></xsl:param>
  <xsl:param name = "FinishedNodeID"></xsl:param>
  <xsl:template match="/">

    <!--<html xmlns:svg="http://www.w3.org/2000/svg" >

      <body style="font-size:9pt;" >-->
    <div  id='divShowFlowShot' style='display: none; position:absolute; width:520px; left: 120; top: 90; z-index:2'>

    </div>
    <svg width="100%" height="100%" version="1.1" xmlns="http://www.w3.org/2000/svg" >
      <defs>
        <marker id="idArrow"
                 viewBox="0 0 20 20" refX="0" refY="10"
                 markerUnits="strokeWidth" markerWidth="3" markerHeight="10"
                 orient="auto">
          <path d="M 0 0 L 20 10 L 0 20 z" fill="purple" stroke="black"/>
        </marker>
        <style type="text/css">
          <![CDATA[
              .touchable { }
              .touchable:hover {  stroke: red; fill: red;stroke-width:1px;}
              .rcntStyle{fill:white;opacity:0;}
              .rcntStyle:hover {  fill:red;stroke-width:1;stroke:rgb(0,0,0);stroke:pink;opacity:0.2;}
              .rcntOn {fill:red;stroke-width:1;stroke:rgb(0,0,0);stroke:pink;opacity:0;}
              .rcntOut {fill:white;opacity:0.5;}
            ]]>
        </style>
      </defs>
      <xsl:apply-templates select="ADDFLOW/NODE">
      </xsl:apply-templates>
      <xsl:apply-templates select="ADDFLOW/LINK">
      </xsl:apply-templates>
    </svg>

    <!--</body>
    </html>-->
  </xsl:template>

  <xsl:template match="NODE">
    <xsl:choose>
      <xsl:when test="contains(@NODEID,'NodeLabel')">
        <xsl:call-template name="Node_TextNew">
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="Node_Text">
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:choose>
      <xsl:when test="USERDATA!=70">
        <xsl:call-template name="Node_Image">
        </xsl:call-template>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="LINK">

    <xsl:choose>
      <xsl:when test="RIGID">

      </xsl:when>
      <xsl:otherwise>

        <path   stroke="gray"  stroke-width="2" fill="none"  marker-end="url(#idArrow)" >
          <xsl:attribute name="d">
            <xsl:text>M </xsl:text>
            <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 40"></xsl:value-of>
            <xsl:text> </xsl:text>
            <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.06 + 10 "></xsl:value-of>
            <xsl:text> l </xsl:text>
            <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@X * 0.055 - EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.055"></xsl:value-of>
            <xsl:text> </xsl:text>
            <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y * 0.055 - EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.055"></xsl:value-of>
          </xsl:attribute >
        </path>

        <text style="fill:black;" >
          <xsl:attribute name="width">
            <xsl:text>120</xsl:text>
          </xsl:attribute >
          <xsl:attribute name="height">
            <xsl:text>30</xsl:text>
          </xsl:attribute >
          <xsl:attribute name="x">
            <xsl:value-of select="LINKTEXT/@X* 0.06 + 25 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="LINKTEXT/@Y* 0.06 + 12"></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="Node_Attribute">
    <xsl:attribute name="id">
      <xsl:text>N</xsl:text>
      <xsl:value-of select="@NODEID"></xsl:value-of>
    </xsl:attribute>
    <xsl:attribute name="style">
      <xsl:text>z-index:20; position:absolute;width:</xsl:text>
      <xsl:value-of select="@WIDTH"></xsl:value-of>
      <xsl:text>;height:</xsl:text>
      <xsl:value-of select="@HEIGHT"></xsl:value-of>
      <xsl:text>;left:</xsl:text>
      <xsl:value-of select="@LEFT * 0.06"></xsl:value-of>
      <xsl:text>;top:</xsl:text>
      <xsl:value-of select="@TOP * 0.06"></xsl:value-of>
      <xsl:text>;curor:hand; </xsl:text>
    </xsl:attribute>
    <v:stroke>
      <xsl:attribute name="dashstyle">
        <xsl:value-of select="DRAWSTYLE"></xsl:value-of>
      </xsl:attribute>
    </v:stroke>
  </xsl:template>

  <xsl:template name="Node_Text">

    <xsl:choose>
      <xsl:when test="contains($FinishedNodeID,@NODEID)">
        <text style="fill:blue;">

          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>
        </text>
      </xsl:when>

      <xsl:when test="@NODEID = $CurrNodeID">
        <text style="fill:blue;">

          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>
        </text>
      </xsl:when>


      <xsl:when test="USERDATA=70">
        <text style="fill:blue;">

          <xsl:attribute name="width">
            <xsl:value-of select="@WIDTH"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="height">
            <xsl:value-of select="@HEIGHT + 30"></xsl:value-of>
          </xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06"></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
      </xsl:when>

      <xsl:otherwise>
        <text style="fill:blue;">
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + 60"></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template name="Node_TextNew">

    <xsl:choose>
      <xsl:when test="contains($FinishedNodeID,substring(@NODEID,11))">
        <text style="fill:#00FFFF;">
          <xsl:attribute name="id">
            <xsl:text>N</xsl:text>
            <xsl:value-of select="@NODEID"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 - 20 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 - 10 "></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>
        </text>

        <rect  class="rcntStyle">
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="id">
            <xsl:text>T</xsl:text>
            <xsl:value-of select="@NODEID"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 - 20 - 20"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 - 10 - 20"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="onclick">
            <xsl:text>jscript:</xsl:text>
            <xsl:text>SelectFlowNode('</xsl:text>
            <xsl:value-of select="LINKTEXT"></xsl:value-of>
            <xsl:text>');</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseover">
            <xsl:text>jscript:GetFlowNodeShot(this,1);</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseout">
            <xsl:text>jscript:hideMe("divShowFlowShot","none");</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="LINKTEXT"></xsl:value-of>        1111111111111111111111111111

        </rect >
      </xsl:when>
      <xsl:when test="substring(@NODEID,11) = $CurrNodeID">
        <text style="fill:red;">
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 - 20 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 - 10 "></xsl:value-of>
          </xsl:attribute >
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
      </xsl:when>



      <xsl:otherwise>
        <text style="fill:blue;">
          <xsl:attribute name="id">
            <xsl:text>N</xsl:text>
            <xsl:value-of select="@NODEID"></xsl:value-of>
          </xsl:attribute >
    
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
        
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 - 20"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 - 10"></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="onmouseover">
            <xsl:text>jscript:GetFlowNodeShot(this,1);</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseout">
            <xsl:text>jscript:hideMe("divShowFlowShot","none");</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>


  <xsl:template name="Node_Image">
    <image>
      <xsl:attribute name="id">
        <xsl:text>IMGEQ</xsl:text>
        <xsl:value-of select="@NODEID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="xlink:href">
        <xsl:text>../Images/FlowDesign/</xsl:text>
        <xsl:value-of select="USERDATA"></xsl:value-of>
        <xsl:text>.ico</xsl:text>
      </xsl:attribute>

      <xsl:attribute name="width">
        <xsl:text>32px</xsl:text>
      </xsl:attribute >
      <xsl:attribute name="height">
        <xsl:text>32px</xsl:text>
      </xsl:attribute >
      <xsl:attribute name="x">
        <xsl:value-of select="@LEFT  * 0.06 + @WIDTH  * 0.06 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y">
        <xsl:value-of select="@TOP * 0.06 + 15 "></xsl:value-of>
      </xsl:attribute >

    </image>

  </xsl:template>

  <xsl:template name="Node_PolyLine">


    <path   stroke="gray"  stroke-width="2" fill="none"  marker-end="url(#idArrow)" >
      <xsl:attribute name="d">
        <xsl:text>M </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@X * 0.06 + 30  "></xsl:value-of>
        <xsl:text> </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y * 0.06  + 20"></xsl:value-of>
        <xsl:text> l </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 - EXTRAPOINTS/POINT[last()]/@X * 0.06+ 30"></xsl:value-of>
        <xsl:text> </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.06 - EXTRAPOINTS/POINT[last()]/@Y * 0.06  "></xsl:value-of>
      </xsl:attribute >
    </path>


  </xsl:template>


</xsl:stylesheet>

