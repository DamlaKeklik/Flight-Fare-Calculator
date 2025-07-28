export default class Form1 extends Form.Designer {

	async DataGrid1_OnRowUpdating(args: Controls.EventArgs.IRowUpdatingEventArgs) {
        console.log(args);
	}

	async DataGrid1_OnRowInserting(args: Controls.EventArgs.IRowInsertingEventArgs) {
        console.log(args);
        console.log(this.DataGrid1.rows);


        this.showMessage("test", "test", "Warning");
        args.cancel = true;
	}
}