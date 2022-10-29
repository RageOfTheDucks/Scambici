$(document).ready(function(){
	$.getJSON(GET_STORES_URL, function(stores) {
		for (i in  stores) {
			$("#store").append('<option value="' + stores[i].StoreId + '">' + stores[i].Address + '</option>');
		}
	});
	$('#user-form').submit(function(event) {
		let formUser = $('#user-form').serializeObject();
		let jsSha = new jsSHA("SHA-512", "TEXT", { numRounds: 1});
		jsSha.update(formUser['PasswordHash']);
		let hash = jsSha.getHash("HEX", {outputUpper: false});
		formUser['PasswordHash'] = hash;

		$.ajax({ 
			type: "POST",
			url: POST_REGISTERUSER_URL,
			data: JSON.stringify(formUser),
			dataType: "json",
			contentType : "application/json",
			success: function(data) {
				alert("Registrazione completata con successo!");
				window.location.href = "/Scambici/Authentication/UserLogin";
			},
			error: function(data) {
				alert("Errore durante la registrazione");
			},
		});
		event.preventDefault();
	});
});
