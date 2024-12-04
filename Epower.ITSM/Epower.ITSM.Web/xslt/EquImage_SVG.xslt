﻿<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:ms="urn:schemas-microsoft-com:xslt"
                xmlns:v="urn:schemas-microsoft-com:vml"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                xmlns:svg="http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd" >
  <xsl:template match="/">

    <div  id='divShowEquShot' style='display: none; position:absolute; width:520px; left: 120; top: 90; z-index:2'>

    </div>

    <svg width="120%" height="120%" version="1.1" xmlns="http://www.w3.org/2000/svg" >
      <defs>
        <marker id="idArrow"
                 viewBox="0 0 20 20" refX="0" refY="10"
                 markerUnits="strokeWidth" markerWidth="3" markerHeight="10"
                 orient="auto">
          <path d="M 0 0 L 20 10 L 0 20 z" fill="purple" stroke="black"/>
        </marker>
      </defs>
      <xsl:apply-templates select="EQURELATION/EQU">
      </xsl:apply-templates>
      <xsl:apply-templates select="EQURELATION/LINK">
      </xsl:apply-templates>
    </svg>


  </xsl:template>

  <xsl:template match="EQU">
    <xsl:call-template name="EQU_Text">
    </xsl:call-template>
    <xsl:choose>
      <xsl:when test="IMAGESRC!=0">
        <xsl:call-template name="EQU_Image">
        </xsl:call-template>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="LINK">
    <!--<line  style="stroke:rgb(99,99,99);stroke-width:2">
      <xsl:attribute name="x1">
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@X * 0.06 + 48"></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y1">
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y * 0.06  "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="x2">
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y2">
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.06 "></xsl:value-of>
      </xsl:attribute >
    </line>-->
    <path   stroke="blue"  stroke-width="3" fill="none"  marker-end="url(#idArrow)" >
      <xsl:attribute name="d">
        <xsl:text>M </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48"></xsl:value-of>
        <xsl:text> </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.06   "></xsl:value-of>
        <xsl:text> l </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@X * 0.06 - EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06"></xsl:value-of>
        <xsl:text> </xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y * 0.06 - EXTRAPOINTS/POINT[count(../*)-1]/@Y * 0.06 "></xsl:value-of>
      </xsl:attribute >
    </path>

    <line  style="stroke:blue;stroke-width:2" >
      <xsl:attribute name="x1">
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y1">
        <xsl:value-of select="EXTRAPOINTS/POINT[1]/@Y * 0.06"></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="x2">
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y2">
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y * 0.06"></xsl:value-of>
      </xsl:attribute >
    </line>

    <xsl:if test="TEXT2">
      <text style="fill:blue;">
        <xsl:attribute name="width">EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48 + 12 </xsl:attribute>
        <xsl:attribute name="height">@HEIGHT + 48</xsl:attribute>
        <xsl:attribute name="x">
          <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48 + 12 "></xsl:value-of>
        </xsl:attribute >
        <xsl:attribute name="y">
          <xsl:value-of select="TEXT2/@Y * 0.06 + 10 "></xsl:value-of>
        </xsl:attribute >
        <xsl:value-of select="TEXT2"></xsl:value-of>
      </text>
    </xsl:if>
    <xsl:if test="TEXT">
      <text style="fill:blue;">
        <xsl:attribute name="width">@WIDTH * 0.06 + 48 + 12 </xsl:attribute>
        <xsl:attribute name="height">@HEIGHT + 48</xsl:attribute>
        <xsl:attribute name="x">
          <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X * 0.06 + 48 + 12 "></xsl:value-of>
        </xsl:attribute >
        <xsl:attribute name="y">
          <xsl:value-of select="TEXT/@Y * 0.06 + 10 "></xsl:value-of>
        </xsl:attribute >
        <xsl:value-of select="TEXT"></xsl:value-of>
      </text>
    </xsl:if>


    <!--<v:PolyLine filled="false" style="position:absolute">
      <xsl:attribute name="POINTS">
        <xsl:for-each select="EXTRAPOINTS/POINT">
          <xsl:value-of select="@X"></xsl:value-of>
          <xsl:text>,</xsl:text>
          <xsl:value-of select="@Y"></xsl:value-of>
          <xsl:text> </xsl:text>
        </xsl:for-each>
      </xsl:attribute>
      <v:stroke>
        <xsl:attribute name="dashstyle">
          <xsl:value-of select="_DRAWSTYLE"></xsl:value-of>
        </xsl:attribute>
      </v:stroke>
    </v:PolyLine>
    <v:Line style="position:absolute; ">
      <xsl:attribute name="from">
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@X"></xsl:value-of>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[count(../*)-1]/@Y"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="to">
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@X"></xsl:value-of>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="EXTRAPOINTS/POINT[last()]/@Y"></xsl:value-of>
      </xsl:attribute>
      <v:stroke dashstyle="solid">
        <xsl:attribute name="dashstyle">
          <xsl:value-of select="_DRAWSTYLE"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="EndArrow">
          <xsl:value-of select="_ARROWDST"></xsl:value-of>
        </xsl:attribute>
      </v:stroke>
    </v:Line>
    <xsl:if test="TEXT">
      <text style="fill:blue;">
        
        <xsl:attribute name="width">@WIDTH</xsl:attribute>
        <xsl:attribute name="height">@HEIGHT + 48</xsl:attribute>
        <xsl:attribute name="x">
          <xsl:value-of select="TEXT/@X * 0.06 "></xsl:value-of>
        </xsl:attribute >
        <xsl:attribute name="y">
          <xsl:value-of select="TEXT/@Y * 0.06"></xsl:value-of>
        </xsl:attribute >
        <xsl:value-of select="TEXT"></xsl:value-of>
      </text>
      
      -->
    <!--<v:Rect filled="f" stroked="f">
        <xsl:attribute name="style">
          <xsl:text>position:absolute; z-index:100; height:3000; width:2500; left:</xsl:text>
          <xsl:value-of select="TEXT/@X"></xsl:value-of>
          <xsl:text>; top:</xsl:text>
          <xsl:value-of select="TEXT/@Y"></xsl:value-of>
        </xsl:attribute>
        <v:Textbox inset="0,0,0,0">
          <xsl:value-of select="TEXT"></xsl:value-of>
        </v:Textbox>
      </v:Rect>-->
    <!--
    </xsl:if>
    <xsl:if test="TEXT2">
      <text style="fill:blue;">
        <xsl:attribute name="width">@WIDTH</xsl:attribute>
        <xsl:attribute name="height">@HEIGHT + 48</xsl:attribute>
        <xsl:attribute name="x">
          <xsl:value-of select="TEXT2/@X * 0.06 "></xsl:value-of>
        </xsl:attribute >
        <xsl:attribute name="y">
          <xsl:value-of select="TEXT2/@Y * 0.06"></xsl:value-of>
        </xsl:attribute >
        <xsl:value-of select="TEXT2"></xsl:value-of>
      </text>
      
      -->
    <!--<v:Rect filled="f" stroked="f">
        <xsl:attribute name="style">
          <xsl:text>position:absolute; z-index:200; height:900; width:2500; left:</xsl:text>
          <xsl:value-of select="TEXT2/@X"></xsl:value-of>
          <xsl:text>; top:</xsl:text>
          <xsl:value-of select="TEXT2/@Y"></xsl:value-of>
        </xsl:attribute>
        <v:Textbox inset="0,0,0,0">
          <xsl:value-of select="TEXT2"></xsl:value-of>
        </v:Textbox>
      </v:Rect>-->
    <!--
    </xsl:if>-->
  </xsl:template>



  <xsl:template name="EQU_Text">
    <text style="fill:blue;">
      <xsl:attribute name="id">
        <xsl:text>EquTD</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="@INDEX"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="equ_id">
        <xsl:value-of select="@EQUID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:text>context-menu-one</xsl:text>
      </xsl:attribute>


      <xsl:attribute name="onclick">
        <xsl:text>javascript:LookEquDetail(this,'</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
        <xsl:text>','</xsl:text>
        <xsl:value-of select="@VERSION"></xsl:value-of>
        <xsl:text>');</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="oncontextmenu">
        <xsl:text>javascript:RightClick(this);</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="RelID">
        <xsl:value-of select="@ID"></xsl:value-of>
      </xsl:attribute>

      <!--<xsl:attribute name="onmouseover">
        <xsl:text>javascript:GetEquShot(this,'</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
        <xsl:text>','</xsl:text>
        <xsl:value-of select="@VERSION"></xsl:value-of>
        <xsl:text>');</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="onmouseout">
        <xsl:text>javascript:hideMe('divShowEquShot','none');</xsl:text>
      </xsl:attribute>-->
      
      <xsl:attribute name="width">@WIDTH + 48 </xsl:attribute>
      <xsl:attribute name="width">@WIDTH + 48 </xsl:attribute>
      <xsl:attribute name="height">@HEIGHT + 48</xsl:attribute>
      <xsl:attribute name="x">
        <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y">
        <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 + 15"></xsl:value-of>
      </xsl:attribute >
      <xsl:value-of select="TEXT"></xsl:value-of>

    </text>

    <!--<v:Rect filled="f" stroked="f">
      <xsl:attribute name="style">
        <xsl:text>position:absolute; z-index:30; width:</xsl:text>
        <xsl:value-of select="@WIDTH"></xsl:value-of>
        <xsl:text>;height:</xsl:text>
        <xsl:value-of select="@HEIGHT + 48"></xsl:value-of>
        <xsl:text>;left:</xsl:text>
        <xsl:value-of select="@LEFT"></xsl:value-of>
        <xsl:text>;top:</xsl:text>
        <xsl:value-of select="@TOP"></xsl:value-of>
        <xsl:text>; </xsl:text>
      </xsl:attribute>
      <table style="font-size:9pt; width:100%; height:100%; ">
        <tr>
          <td id="tdItem" style="width:100%; height:100%; color:blue; text-align:center; vertical-align:bottom; ">
            <xsl:attribute name="onmouseover">
              <xsl:text>javascript:GetEquShot(this);</xsl:text>
            </xsl:attribute>

            <xsl:attribute name="onmouseout">
              <xsl:text>javascript:hideMe("divShowEquShot","none");</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="onclick">
              <xsl:text>javascript:LookEquDetail(this);</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="oncontextmenu">
              <xsl:text>javascript:RightClick(this);</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="id">
              <xsl:text>EquTD</xsl:text>
              <xsl:value-of select="@EQUID"></xsl:value-of>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="@INDEX"></xsl:value-of>
            </xsl:attribute>
            <xsl:attribute name="RelID">
              <xsl:value-of select="@ID"></xsl:value-of>
            </xsl:attribute>
            <a>
              <xsl:attribute name="href">
                <xsl:text>#</xsl:text>
              </xsl:attribute>
              <xsl:attribute name="name">
                <xsl:text>EquLnk</xsl:text>
                <xsl:value-of select="@EQUID"></xsl:value-of>
              </xsl:attribute>
              <xsl:value-of select="TEXT"></xsl:value-of>
            </a>
          </td>

        </tr>
      </table>
    </v:Rect>-->
  </xsl:template>

  <xsl:template name="EQU_Image">
    <image>
      <xsl:attribute name="id">
        <xsl:text>IMGEQ</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="code">
        <xsl:text>IMGEQ</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="xlink:href">
        <xsl:value-of select="IMAGESRC"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:text>context-menu-one equ_object</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="RelID">
        <xsl:value-of select="@ID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="width">
        <xsl:text>48px</xsl:text>
      </xsl:attribute >
      <xsl:attribute name="height">
        <xsl:text>48px</xsl:text>
      </xsl:attribute >
      <xsl:attribute name="x">
        <xsl:value-of select="@LEFT  * 0.06 + @WIDTH  * 0.06 "></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="y">
        <xsl:value-of select="@TOP * 0.06 + 10"></xsl:value-of>
      </xsl:attribute >
      <xsl:attribute name="onclick">        
        <xsl:text>javascript:LookEquDetail(this,'</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
        <xsl:text>','</xsl:text>
        <xsl:value-of select="@VERSION"></xsl:value-of>
        <xsl:text>');</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="onmouseover">
        <xsl:text>javascript:GetEquShot(this);</xsl:text>
      </xsl:attribute>

      <xsl:attribute name="onmouseout">
        <xsl:text>javascript:hideMe("divShowEquShot","none");</xsl:text>
      </xsl:attribute>
    </image>

    <xsl:if test="@DISPLAYERRORICO[.='0']">
      <image>        
        <xsl:attribute name="id">
          <xsl:text>IMG_HIDDEN_</xsl:text>
          <xsl:value-of select="@EQUID"></xsl:value-of>
        </xsl:attribute>
        <xsl:attribute name="xlink:href">
          <xsl:text>../Images/x.gif</xsl:text>
        </xsl:attribute>
        <xsl:attribute name="style">
          <xsl:text>display:none;</xsl:text>
        </xsl:attribute>
      </image>
    </xsl:if>

    <!--<v:image>
      <xsl:attribute name="id">
        <xsl:text>IMG</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="src">
        <xsl:value-of select="IMAGESRC"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:text>position:absolute; z-index:20; width:480; height:480; left:</xsl:text>
        <xsl:value-of select="@LEFT + @WIDTH div 2 - 240"></xsl:value-of>
        <xsl:text>; top:</xsl:text>
        <xsl:value-of select="@TOP + 15"></xsl:value-of>
      </xsl:attribute>
    </v:image>-->

  </xsl:template>

  


</xsl:stylesheet>

