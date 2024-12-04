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
    <html>




      <body style="font-size:9pt;" >
        <form name="flow_Chart" id="flow_Chart">
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
            </defs>
            <xsl:apply-templates select="ADDFLOW/NODE">
            </xsl:apply-templates>
            <xsl:apply-templates select="ADDFLOW/LINK">
            </xsl:apply-templates>
          </svg>
        </form>
      </body>
    </html>
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
        <path   stroke="blue"  stroke-width="3" fill="none"  marker-end="url(#idArrow)" >
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

        <text style="fill:red;" >
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
          <v:Rect filled="f" stroked="f">
            <xsl:attribute name="style">
              <xsl:text>position:absolute; z-index:100; height:3000; width:30000; left:</xsl:text>
              <xsl:value-of select="TEXT/@X"></xsl:value-of>
              <xsl:text>; top:</xsl:text>
              <xsl:value-of select="TEXT/@Y"></xsl:value-of>
            </xsl:attribute>
            <v:Textbox inset="0,0,0,0">
              <xsl:value-of select="TEXT"></xsl:value-of>
            </v:Textbox>
          </v:Rect>
        </xsl:if>-->
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
      <xsl:value-of select="@LEFT"></xsl:value-of>
      <xsl:text>;top:</xsl:text>
      <xsl:value-of select="@TOP"></xsl:value-of>
      <xsl:text>;curor:hand; </xsl:text>
    </xsl:attribute>
    <v:stroke>
      <xsl:attribute name="dashstyle">
        <xsl:value-of select="DRAWSTYLE"></xsl:value-of>
      </xsl:attribute>
    </v:stroke>
  </xsl:template>

  <xsl:template name="Node_Text">
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
    <!--<v:Rect filled="f" stroked="f">
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
      <table style="font-size:9pt; width:100%; height:100%; " >
        <xsl:attribute name="id">
          <xsl:text>Table</xsl:text>
          <xsl:value-of select="@NODEID"></xsl:value-of>
        </xsl:attribute>
        <tr>
          <xsl:choose>
            <xsl:when test="contains($FinishedNodeID,@NODEID)">
              <td style="width:100%; height:100%; color:green; text-align:center; vertical-align:bottom; ">

                <xsl:attribute name="id">
                  <xsl:text>T</xsl:text>
                  <xsl:value-of select="@NODEID"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </xsl:when>
            <xsl:when test="@NODEID = $CurrNodeID">
              <td style="width:100%; height:100%; color:red; text-align:center; vertical-align:bottom; ">
                <xsl:attribute name="id">
                  <xsl:text>T</xsl:text>
                  <xsl:value-of select="@NODEID"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </xsl:when>
            <xsl:when test="USERDATA=70">
              <td style="width:100%; height:100%; color:blue; text-align:left; vertical-align:top; ">
                <xsl:attribute name="id">
                  <xsl:text>T</xsl:text>
                  <xsl:value-of select="@NODEID"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </xsl:when>
            <xsl:otherwise>
              <td style="width:100%; height:100%; color:blue; text-align:center; vertical-align:bottom; ">
                <xsl:attribute name="id">
                  <xsl:text>T</xsl:text>
                  <xsl:value-of select="@NODEID"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </xsl:otherwise>
          </xsl:choose>

        </tr>
      </table>
    </v:Rect>-->
  </xsl:template>


  <xsl:template name="Node_TextNew">

    <xsl:choose>
      <xsl:when test="contains($FinishedNodeID,substring(@NODEID,11))">
        <text style="fill:blue;">
          <xsl:attribute name="width">80</xsl:attribute>
          <xsl:attribute name="height">30</xsl:attribute>
          <xsl:attribute name="x">
            <xsl:value-of select="@LEFT * 0.06 + @WIDTH * 0.06 - 20 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="y">
            <xsl:value-of select="@TOP * 0.06 + @HEIGHT * 0.06 - 10 "></xsl:value-of>
          </xsl:attribute >
          <xsl:attribute name="onclick">
            <xsl:text>jscript:onClickNew();</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseover">
            <xsl:text>jscript:onMouseOverSelectNew(1);</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseout">
            <xsl:text>jscript:onMouseOutSelectNew();</xsl:text>
          </xsl:attribute>
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
        <!--<v:Rect filled="f" stroked="f">
          <xsl:attribute name="style">
            <xsl:text>position:absolute; z-index:30; width:</xsl:text>
            <xsl:value-of select="@WIDTH"></xsl:value-of>
            <xsl:text>;height:</xsl:text>
            <xsl:value-of select="@HEIGHT + 30"></xsl:value-of>
            <xsl:text>;left:</xsl:text>
            <xsl:value-of select="@LEFT"></xsl:value-of>
            <xsl:text>;top:</xsl:text>
            <xsl:value-of select="@TOP - 200 "></xsl:value-of>
            <xsl:text>; </xsl:text>
          </xsl:attribute>
          <table style="font-size:9pt; width:100%; height:100%; ">
            <tr>
              <td style="width:100%; height:100%; color:green; text-align:center; vertical-align:top; ">
                <xsl:attribute name="onclick">
                  <xsl:text>jscript:onClickNew();</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="onmouseover">
                  <xsl:text>jscript:onMouseOverSelectNew(1);</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="onmouseout">
                  <xsl:text>jscript:onMouseOutSelectNew();</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="id">
                  <xsl:text>L</xsl:text>
                  <xsl:value-of select="substring(@NODEID,11)"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </tr>
          </table>
        </v:Rect>-->
      </xsl:when>
      <xsl:when test="substring(@NODEID,11) = $CurrNodeID">
        <text style="fill:blue;">

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
        
        <!--<v:Rect filled="f" stroked="f">
          <xsl:attribute name="style">
            <xsl:text>position:absolute; z-index:30; width:</xsl:text>
            <xsl:value-of select="@WIDTH"></xsl:value-of>
            <xsl:text>;height:</xsl:text>
            <xsl:value-of select="@HEIGHT + 30"></xsl:value-of>
            <xsl:text>;left:</xsl:text>
            <xsl:value-of select="@LEFT"></xsl:value-of>
            <xsl:text>;top:</xsl:text>
            <xsl:value-of select="@TOP - 200"></xsl:value-of>
            <xsl:text>; </xsl:text>
          </xsl:attribute>
          <table style="font-size:9pt; width:100%; height:100%; ">
            <tr>
              <td style="width:100%; height:100%; color:red; text-align:center; vertical-align:top; ">

                <xsl:attribute name="id">
                  <xsl:text>L</xsl:text>
                  <xsl:value-of select="substring(@NODEID,11)"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </tr>
          </table>
        </v:Rect>-->
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
        
          <xsl:attribute name="onclick">
            <xsl:text>jscript:onClickNew();</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseover">
            <xsl:text>jscript:onMouseOverSelectNew(1);</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="onmouseout">
            <xsl:text>jscript:onMouseOutSelectNew();</xsl:text>
          </xsl:attribute>
          <xsl:text>9999</xsl:text>
          <xsl:value-of select="LINKTEXT"></xsl:value-of>

        </text>
        
        <!--<v:Rect filled="f"  stroked="f">
          <xsl:attribute name="style">
            <xsl:text>position:absolute; z-index:30; width:</xsl:text>
            <xsl:value-of select="@WIDTH"></xsl:value-of>
            <xsl:text>;height:</xsl:text>
            <xsl:value-of select="@HEIGHT"></xsl:value-of>
            <xsl:text>;left:</xsl:text>
            <xsl:value-of select="@LEFT"></xsl:value-of>
            <xsl:text>;top:</xsl:text>
            <xsl:value-of select="@TOP - 200"></xsl:value-of>
            <xsl:text>; </xsl:text>
          </xsl:attribute>
          <table style="font-size:9pt; width:100%; height:100%; ">
            <tr>
              <td style="width:100%; height:100%; color:blue; text-align:center; vertical-align:top; ">
                <xsl:attribute name="onclick">
                  <xsl:text>jscript:onClickNew();</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="onmouseover">
                  <xsl:text>jscript:onMouseOverSelectNew(0);</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="onmouseout">
                  <xsl:text>jscript:onMouseOutSelectNew();</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="id">
                  <xsl:text>L</xsl:text>
                  <xsl:value-of select="substring(@NODEID,11)"></xsl:value-of>
                </xsl:attribute>
                <xsl:value-of select="TEXT"></xsl:value-of>
              </td>
            </tr>
          </table>
        </v:Rect>-->
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
    <!--<v:image>
      <xsl:attribute name="id">
        <xsl:text>IMG</xsl:text>
        <xsl:value-of select="@NODEID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="src">
        <xsl:text>../Images/FlowDesign/</xsl:text>
        <xsl:value-of select="USERDATA"></xsl:value-of>
        <xsl:text>.ico</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:text>position:absolute; z-index:20; width:480; height:480; left:</xsl:text>
        <xsl:value-of select="@LEFT + @WIDTH div 2 - 240"></xsl:value-of>
        <xsl:text>; top:</xsl:text>
        <xsl:value-of select="@TOP + 15"></xsl:value-of>
      </xsl:attribute>
    </v:image>-->
  </xsl:template>

  <xsl:template name="Node_PolyLine">

    <path   stroke="blue"  stroke-width="3" fill="none"  marker-end="url(#idArrow)" >
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
    
    <!--<v:PolyLine>
      <xsl:attribute name="id">
        <xsl:text>PolyLine</xsl:text>
        <xsl:value-of select="@NODEID"></xsl:value-of>
      </xsl:attribute>
      <xsl:attribute name="filled">
        <xsl:text>false</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="style">
        <xsl:text>position:absolute;</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="StrokeColor">
        <xsl:text>red</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="Points">
        <xsl:text>0,0 0,0</xsl:text>
      </xsl:attribute>
      <v:stroke>
        <xsl:attribute name="EndArrow">
          <xsl:text>Classic</xsl:text>
        </xsl:attribute>
      </v:stroke>
    </v:PolyLine>-->
  </xsl:template>


</xsl:stylesheet>

