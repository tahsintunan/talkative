import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-empty-result-overlay',
  templateUrl: './empty-result-overlay.component.html',
  styleUrls: ['./empty-result-overlay.component.css'],
})
export class EmptyResultOverlayComponent implements OnInit {
  @Input() type: 'noSearchResult' | 'noData' | 'noDataAlt' | 'endReached' =
    'noSearchResult';
  @Input() title: string = '';
  @Input() message: string = '';

  constructor() {}

  ngOnInit(): void {}
}
