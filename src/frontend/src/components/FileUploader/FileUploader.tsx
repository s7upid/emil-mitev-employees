import React, { useState } from "react";
import { uploadEmployeeFile } from "../../api/employees";

import "./FileUploader.scss";

interface Props {
  onUploadSuccess: () => void;
}

const FileUploader: React.FC<Props> = ({ onUploadSuccess }) => {
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState("");

  const [success, setSuccess] = useState(false);

  const handleUpload = async () => {
    if (!file) {
      setError("Please select a file.");
      setSuccess(false);
      return;
    }

    try {
      await uploadEmployeeFile(file);
      onUploadSuccess();
      setError("");
      setSuccess(true);
    } catch (err: any) {
      setError("Upload failed. Please try again.");
      setSuccess(false);
    }
  };

  return (
    <div className="file-uploader">
      <input
        data-testid="file-input"
        type="file"
        accept=".csv"
        onChange={(e) => {
          setFile(e.target.files?.[0] || null);
          setSuccess(false);
          setError("");
        }}
      />
      <button data-testid="upload-button" onClick={handleUpload}>
        Upload
      </button>
      {error && <p className="error-message">{error}</p>}
      {success && <p className="success-message">Upload successful</p>}
    </div>
  );
};

export default FileUploader;
