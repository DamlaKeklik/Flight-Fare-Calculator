export default class Form1 extends Form.Designer {

	async DataGrid1_OnRowUpdating(args: Controls.EventArgs.IRowUpdatingEventArgs) {
        console.log(args);
		args.rows.forEach(
			row =>{
				if(row.SEHIRLER.toLocaleLowerCase("tr")==args.newRow.SEHIRLER.toLocaleLowerCase("tr")){
					this.showMessage("HATA","Aynı ada sahip şehir var!","Error")
					args.cancel=true
				}
			}
		)
	}

	async DataGrid1_OnRowUpdated(args: Controls.EventArgs.IRowUpdatedEventArgs) {
        console.log(args);
	}

	async DataGrid1_OnRowInserting(args: Controls.EventArgs.IRowInsertingEventArgs) {
        console.log(args);
        args.rows.forEach(
			row => {
				if (row.SEHIRLER.toLocaleLowerCase("tr")==args.row.SEHIRLER.toLocaleLowerCase("tr")){
					this.showMessage("HATA","Aynı ada sahip şehir var!","Error")
					args.cancel=true
				}
			
			}
		)
	}
}