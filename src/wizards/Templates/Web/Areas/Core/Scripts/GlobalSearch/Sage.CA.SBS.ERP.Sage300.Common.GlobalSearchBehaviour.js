"use strict";var globalSearchUI=globalSearchUI||{};globalSearchUI={globalSearchModel:{data:ko.observableArray()},dataSourceConfig:{transport:{read:function(n){globalSearchUI.search(n)}},pageSize:globalSearchPageSize,serverPaging:!0,schema:{data:function(n){return n.Data},total:function(n){return n.Total}}},init:function(){globalSearchUI.initButton();globalSearchUI.initListView();globalSearchUI.initTreeView()},initTreeView:function(){$("#searchableEntityTreeView").kendoTreeView({checkboxes:{checkChildren:!0},dataSource:entitySchemaList});var t=$("#searchableEntityTreeView").data("kendoTreeView"),n=t.dataItem(".k-item:first");n&&(n.set("checked",!0),n.items&&n.items.length>0&&t.expandTo(n.items[n.items.length-1]))},initListView:function(){var n=new kendo.data.DataSource(globalSearchUI.dataSourceConfig);$("#bodySearchResultListView").kendoListView({dataSource:n,template:kendo.template($("#template").html()),autoBind:!1,dataBound:globalSearchUI.initSearchResult});$("#searchResultPager").kendoPager({dataSource:n,numeric:!0,autoBind:!1})},initSearchResult:function(){$(".item-ID").click(function(){var n=this.dataset.webdetailinfo.split(";"),t;if(window.parent.globalSearchDrillDownParameter="",(n.length-1)%2==0)for(t=1;t<n.length;t++)window.parent.globalSearchDrillDownParameter!==""&&(window.parent.globalSearchDrillDownParameter+="&"),window.parent.globalSearchDrillDownParameter+=n[t]+"="+n[++t];$("a[data-menuid="+n[0]+"]",window.parent.document.body)[0].click()})},initButton:function(){$("#btnSearchFind").click(function(){var n=[];globalSearchUI.getCheckedEntity($("#searchableEntityTreeView").data("kendoTreeView").dataSource.view(),n);n.length==0?sg.utls.showKendoMessageDialog(function(){},globalSearchResource.noSelection):$("#searchResultPager").data().kendoPager.page()===0?$("#bodySearchResultListView").data().kendoListView.dataSource.read():$("#searchResultPager").data().kendoPager.page(1)});$("#globalSearchBox").keyup(function(n){n.keyCode===13&&$("#btnSearchFind").click()});$("#btnResetCompany").click(function(){sg.utls.showKendoConfirmationDialog(function(){globalSearchRepository.ResetCompany({},function(){})},function(){},globalSearchResource.resetConfirm)})},initAutoComplete:function(){var n='# for (var i = 1; i < data.Path.length; i++) { ## if (i > 1) { # - # } ##: data.Path[i] ## } ## for (var j = 0; j < data.Fields.length; j++) { # - #: data["FieldName-" + data.Fields[j]] #: #: data[data.Fields[j]] ## } #',t=$("#globalSearchBox").kendoAutoComplete({minLength:2,template:'<span class="k-state-default" style="background-image: url(\'../../../Assets/images/search/#:data.Type#.png\')"><\/span><span class="k-state-default" title="'+n+'"><h3>#: data["FieldName-"+data.Header] #: #: data[data.Header] #<\/h3><p>'+n+"<\/p><\/span>",dataSource:{serverFiltering:!0,transport:{read:{url:function(){return sg.utls.url.buildUrl("Core","GlobalSearch","Search")},data:function(){return{query:$("#globalSearchBox").val()}}}}},dataTextField:"customer_number",height:408,clearButton:!1}).data("kendoAutoComplete")},search:function(n){var t,i;$(".main-body").hasClass("result-active")||($(".main-body").addClass("result-active"),$(".body-search-result").show());t=[];globalSearchUI.getCheckedEntity($("#searchableEntityTreeView").data("kendoTreeView").dataSource.view(),t);i={query:$("#globalSearchBox").val(),entities:t,pageSize:n.data.pageSize,pageNumber:n.data.page};globalSearchRepository.Search(i,function(t){t&&(t.UserMessage?sg.utls.showMessage(t):n.success(t))})},getCheckedEntity:function(n,t){for(var i=0;i<n.length;i++)n[i].checked&&!n[i].header&&t.push(n[i].id),n[i].hasChildren&&globalSearchUI.getCheckedEntity(n[i].children.view(),t)}};$(document).ready(function(){globalSearchUI.init();$(window).bind("beforeunload",function(){$(parent.document.getElementById("globalSearch")).removeClass("active")})});