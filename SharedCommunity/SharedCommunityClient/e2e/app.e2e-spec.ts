import { SharedCommunityClientPage } from './app.po';

describe('shared-community-client App', () => {
  let page: SharedCommunityClientPage;

  beforeEach(() => {
    page = new SharedCommunityClientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
