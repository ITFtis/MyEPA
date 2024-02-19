var image_file=new Array();
var id, idx;
var total;
var loop_on=0;
var loop_num = 12;
var loop_hour = 0;
var num_perhour = 2;

function SelectArea(d) {

	$("#sel").empty();
	$("#sel").addOption(d, false);
	$("#sel option:eq(0)").attr('selected', 'selected');

	$("#sel").trigger("change"); 
}

function Stop_Loop(){
		if(id)
			clearTimeout(id);
}
function PrepareLoop(d) {
//alert(d);
	if(d==0) {
		loop_on = 0;
		loop_hour = 0;
		k = $("#sel").val();
		$("#im").attr("src", "http://www.cwb.gov.tw"+k);
		Stop_Loop();
	}
	else {
		loop_hour = d;
		loop_num = d*num_perhour;
		Stop_Loop();
		loop_on = 1;
		k = $("#sel").val();
		k = $("#sel").attr("selectedIndex")
//		alert(k);
	    max = $('#sel option').size()-1;
		beginindex = k + loop_num - 1;

		if(beginindex > max) beginindex = max;
		from = beginindex;
		to = k;
//		alert(from+"...."+to);

		total=from-to+1;
		ii=0;
		for(i=from;i>=to;i--) {
				a = new Image();
				fn=$("#sel option")[i].value;
				a.src="http://www.cwb.gov.tw"+fn;
				image_file[ii]= a;
				ii=ii+1;
		} 

	
		idx = 0;
		Loop();
	}
}


function Loop() {
//		alert("Loop...."+idx);
//		$("#sel option:eq("+idx+")").attr('selected', 'selected');
//		src = $("#sel").val();
//		alert(src);
		$("#im").attr("src", image_file[idx].src);
		idx=(idx+1)%total;
		id = setTimeout("Loop()",500);
}
