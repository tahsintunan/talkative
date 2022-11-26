import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-hashtag-renderer',
  templateUrl: './hashtag-renderer.component.html',
  styleUrls: ['./hashtag-renderer.component.css'],
})
export class HashtagRendererComponent implements OnInit {
  @Input() text?: string;
  @Output() onHashtagClick = new EventEmitter<string>();

  constructor() {}

  ngOnInit(): void {}

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      event.stopPropagation();
      this.onHashtagClick.emit(event.target.innerText.trim());
    }
  }
}
