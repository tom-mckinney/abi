import * as React from "react";

export default class ContentTagger extends React.Component {
  previewUrl: string;

  componentDidMount() {
    const previewUrl = document.getElementById("renderPreviewUrl");
    if (previewUrl) {
      this.previewUrl = previewUrl.getAttribute("data-value");
    }
  }
  render() {
    return (
      <div>Wumbo</div>
    );
  }
}
