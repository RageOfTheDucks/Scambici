$(document).ready(function(){
	var name = $.session.get("name");
	var surname = $.session.get("surname");

	$("#fullname").html(name +" "+ surname);
});
