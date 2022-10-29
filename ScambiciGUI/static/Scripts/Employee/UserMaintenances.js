$(document).ready(function(){
	var userMaintenaceAuth = {};
	let timestamp = $.getTimestamp();
	userMaintenaceAuth['EmployeeId'] = $.session.get("employeeId");
	userMaintenaceAuth['Signature'] = $.calculateSignature(timestamp);
	userMaintenaceAuth['Timestamp'] = timestamp;

	$.ajax({ 
		type: "POST",
		url: POST_GETUSERMAINTREQ_URL,
		data: JSON.stringify(userMaintenaceAuth),
		dataType: "json",
		contentType : "application/json",
		success: function(userMaintenances){
			if (userMaintenances.length === 0) {
				$("#userMaintenances").html('<div class="mt-5 h1 text-secondary text-center">Nessuna manutenzione in sospeso</div>')
			}
			else {
				for (i in  userMaintenances) {
					$("#userMaintenances").append('<div class="container list-container bg-body text-head rounded-lg shadow"><div class="text-head my-2">Manutenzione richiesta da <br class="d-sm-none" \><span class="text-lg-head font-weight-bold">'+ userMaintenances[i].CreatedBy.Name +' '+ userMaintenances[i].CreatedBy.Surname +'</span><br/><span class="font-weight-bold">Dettagli: </span>'+ userMaintenances[i].FaultDescription +'</div><form class="userMaintenanceForm"><input type="hidden" name="TaskId" value="'+ userMaintenances[i].TaskId +'"/><div class="mt-2"><button type="submit" class="btn btn-block btn-head mt-2">Prendi in carico</button></div></form></div>');
				}
			}
		},
		error: function(data){
			alert("Errore durante il caricamento delle richieste");
		},
	});
});

$(document).on('submit', 'form', function(event){
	var assignUserMaintenance = {};
	let timestamp = $.getTimestamp();
	assignUserMaintenance['EmployeeId'] = $.session.get("employeeId");
	assignUserMaintenance['Signature'] = $.calculateSignature(timestamp);
	assignUserMaintenance['Timestamp'] = timestamp;
	assignUserMaintenance['TaskId'] = $(this).children('input[name=TaskId]').val();


	$.ajax({ 
		type: "POST",
		url: POST_ASSIGN_USERMAINT_URL,
		data: JSON.stringify(assignUserMaintenance),
		dataType: "json",
		contentType : "application/json",
		success: function(data) {
			window.location.href = "/Scambici/Employee/HomeEmployee";
		},
		error: function(data) {
			alert("Errore durante la presa in carico della manutenzione");
		},
	});
	event.preventDefault();
});
