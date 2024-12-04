$(document).ready(function() {
          $('#cmdOpenRelChart').click(function(){
              var equid = epower.equid;
              
              if($.browser.msie) { 
                  url = "frm_Equ_RelChartView.aspx?newWin=true&id=" + equid;
              } else {
                  url = "frm_Equ_RelChartView_SVG.aspx?newWin=true&id=" + equid;
              }
              
              url = url + '&search_key=default';
              window.open(url, "", "scrollbars=yes,status=yes ,resizable=yes,width=800,height=600");
          });
    });    