<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:ms="urn:schemas-microsoft-com:xslt" xmlns:v="urn:schemas-microsoft-com:vml">
  <xsl:template match="/">
    <html>
      <style>
        v\:* { Behavior: url(#default#VML) }
      </style>
      <body style="font-size:9pt;" >
        <div  id='divShowEquShot' style='display: none; position:absolute; width:520px; left: 120; top: 90; z-index:2'>

        </div>
        <v:group id="flowchartshow" style="position:absolute;width:1000px;height:1000px;" coordsize="15000,15000">
          <xsl:apply-templates select="EQURELATION/EQU">
          </xsl:apply-templates>
          <xsl:apply-templates select="EQURELATION/LINK">
          </xsl:apply-templates>
        </v:group>
      </body>
    </html>
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
    <v:PolyLine filled="false" style="position:absolute">
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
        <xsl:attribute name="StartArrow">
          <xsl:value-of select="_ARROWDST"></xsl:value-of>
        </xsl:attribute>
      </v:stroke>
    </v:Line>
    <xsl:if test="TEXT">
      <v:Rect filled="f" stroked="f">
        <xsl:attribute name="style">
          <xsl:text>position:absolute; z-index:100; height:3000; width:2500; left:</xsl:text>
          <xsl:value-of select="TEXT/@X"></xsl:value-of>
          <xsl:text>; top:</xsl:text>
          <xsl:value-of select="TEXT/@Y"></xsl:value-of>
        </xsl:attribute>
        <v:Textbox inset="0,0,0,0">
          <xsl:attribute name="onclick">
            <xsl:text>javascript:LookChangeDetail('</xsl:text>
            <xsl:value-of select="@FLOWID"></xsl:value-of>
            <xsl:text>');</xsl:text>
          </xsl:attribute>

          <xsl:attribute name="id">
            <xsl:text>EquTD</xsl:text>
            <xsl:value-of select="@EQUID"></xsl:value-of>
          </xsl:attribute>
          <a>
            <xsl:attribute name="href">
              <xsl:text>#</xsl:text>
            </xsl:attribute>
            <xsl:attribute name="name">
              <xsl:text>EquLnk</xsl:text>
              <xsl:value-of select="@FLOWID"></xsl:value-of>
            </xsl:attribute>
            <xsl:value-of select="TEXT"></xsl:value-of>
            
          </a>
          <xsl:value-of select="@TEXTSAVE"></xsl:value-of>
        </v:Textbox>
        
      </v:Rect>
      
    </xsl:if>
  </xsl:template>



  <xsl:template name="EQU_Text">
    <v:Rect filled="f" stroked="f">
      <xsl:attribute name="style">
        <xsl:text>position:absolute; z-index:30; width:</xsl:text>
        <xsl:value-of select="@WIDTH"></xsl:value-of>
        <xsl:text>;height:</xsl:text>
        <xsl:value-of select="@HEIGHT + 30"></xsl:value-of>
        <xsl:text>;left:</xsl:text>
        <xsl:value-of select="@LEFT"></xsl:value-of>
        <xsl:text>;top:</xsl:text>
        <xsl:value-of select="@TOP"></xsl:value-of>
        <xsl:text>; </xsl:text>
      </xsl:attribute>
      <table style="font-size:9pt; width:100%; height:100%; ">
        <tr>
          <td style="width:100%; height:100%; color:blue; text-align:center; vertical-align:bottom; "  >
            <xsl:attribute name="onmouseover">
              <xsl:text>javascript:GetEquShot(this,'</xsl:text>
              <xsl:value-of select="@EQUID"></xsl:value-of>
              <xsl:text>','</xsl:text>
              <xsl:value-of select="@VERSION"></xsl:value-of>
              <xsl:text>');</xsl:text>
            </xsl:attribute>

            <xsl:attribute name="onmouseout">
              <xsl:text>javascript:hideMe('divShowEquShot','none');</xsl:text>
              
            </xsl:attribute>

            <xsl:attribute name="onclick">
              <xsl:text>javascript:LookEquDetail(this,'</xsl:text>
              <xsl:value-of select="@EQUID"></xsl:value-of>
              <xsl:text>','</xsl:text>
              <xsl:value-of select="@VERSION"></xsl:value-of>
              <xsl:text>');</xsl:text>
            </xsl:attribute>
            
            <xsl:attribute name="id">
              <xsl:text>EquTD</xsl:text>
              <xsl:value-of select="@EQUID"></xsl:value-of>
              <xsl:value-of select="@VERSION"></xsl:value-of>
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
    </v:Rect>
  </xsl:template>

  <xsl:template name="EQU_Image">
    <v:image>
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
    </v:image>
    <!--<xsl:if test="@DISPLAYERRORICO[.='0']">    
        <v:image>
      <xsl:attribute name="id">
        <xsl:text>IMG_HIDDEN_</xsl:text>
        <xsl:value-of select="@EQUID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="src">       
        <xsl:text>../Images/x.gif</xsl:text>
      </xsl:attribute>   
      <xsl:attribute name="style">
        <xsl:text>position:absolute; z-index:20; width:480; height:480; left:</xsl:text>
        <xsl:value-of select="@LEFT + @WIDTH div 2 - 240 + 400"></xsl:value-of>
        <xsl:text>; top:</xsl:text>
        <xsl:value-of select="@TOP + 15 - 200"></xsl:value-of>
      </xsl:attribute>
    </v:image>
    </xsl:if>-->
  
</xsl:template>




</xsl:stylesheet>

