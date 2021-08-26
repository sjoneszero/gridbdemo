import { BackendService } from './../../services/backend.service';
import { Component } from '@angular/core';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'upload-modal',
  templateUrl: './uploadmodal.component.html'
})
export class UploadmodalComponent {
  closeResult = '';
  file: File = undefined;
  success: boolean;
  error: boolean;
  errormessage: string;
  uploading: boolean;
  description: string;


  constructor(private modalService: NgbModal, private backendService : BackendService) {}

  resetFlags(): void {
    this.success = false;
    this.error = false;
    this.uploading = false;
  }

  onChangeDescription(event: any) {
    this.description = event.target.value;
}
  onChangeFile(event: any) {
    this.file = event.target.files[0];
}

// OnClick of button Upload
  onUpload() {
      this.uploading = true;
      console.log(this.file);
        const formData = new FormData();
        formData.append("file", this.file, this.file.name);
        formData.append("description", this.description)

      this.backendService.uploadCsv(formData).subscribe(
          (response: any) => {
            this.uploading = false;
              this.success = true;
          },
          (error) => {
            this.uploading = false;
            this.error = true;
            this.errormessage = "Upload Failed. Please check the formatting of your CSV file";
          }

      );
  }
  open(content: any) {
    this.resetFlags()
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
  }
}




