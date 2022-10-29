$(document).ready(function(){
	var rentalInterruptAuth = {};
	let timestamp = $.getTimestamp();
	rentalInterruptAuth['EmployeeId'] = $.session.get("employeeId");
	rentalInterruptAuth['Signature'] = $.calculateSignature(timestamp);
	rentalInterruptAuth['Timestamp'] = timestamp;

	$.ajax({ 
		type: "POST",
		url: POST_GETRENTALINTERRUPTREQ_URL,
		data: JSON.stringify(rentalInterruptAuth),
		dataType: "json",
		contentType : "application/json",
		success: function(interruptions){
			if (interruptions.length === 0) {
				$("#interruptions").html('<div class="mt-5 h1 text-secondary text-center">Nessuna richiesta in sospeso</div>')
			}
			else {
				for (i in  interruptions) {
					$("#interruptions").append('<div class="container list-container bg-body text-head rounded-lg shadow"><div class="text-head my-2">Interruzione noleggio richiesta da <br/><span class="text-lg-head font-weight-bold">'+ interruptions[i].CreatedBy.Name +' '+ interruptions[i].CreatedBy.Surname +'</span></div><form class="rental-interruption-confirmation"><input type="hidden" name="RentalInterruptionId" value="'+ interruptions[i].RentalInterruptionId +'"/><div class="mt-2"><button type="submit" class="btn btn-block btn-head mt-2">Concludi</button></div></form></div>');
				}
			}
		},
		error: function(data){
			alert("Errore durante il caricamento delle richieste");
		},
	});
});

$(document).on('submit', 'form', function(event){
	var rentalInterruptionConfirmation = {};
	let timestamp = $.getTimestamp();
	rentalInterruptionConfirmation['EmployeeId'] = $.session.get("employeeId");
	rentalInterruptionConfirmation['Signature'] = $.calculateSignature(timestamp);
	rentalInterruptionConfirmation['Timestamp'] = timestamp;
	rentalInterruptionConfirmation['RentalInterruptionId'] = $(this).children('input[name=RentalInterruptionId]').val();


	$.ajax({ 
		type: "POST",
		url: POST_RENTALINTERRUPTCONF_URL,
		data: JSON.stringify(rentalInterruptionConfirmation),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			alert("Interruzione noleggio confermata con successo");
		},
		error: function(data) {
			alert("Errore durante la conferma noleggio");
		},
	});
	event.preventDefault();
});