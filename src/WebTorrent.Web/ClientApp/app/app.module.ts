import { NgModule, ErrorHandler } from "@angular/core";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import 'bootstrap';
import { TranslateModule, TranslateLoader } from "@ngx-translate/core";
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ToastyModule } from 'ng2-toasty';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { PopoverModule } from "ngx-bootstrap/popover";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { ChartsModule } from 'ng2-charts';

import { AppRoutingModule } from './app-routing.module';
import { AppErrorHandler } from './app-error.handler';
import { AppTitleService } from './services/app-title.service';
import { AppTranslationService, TranslateLanguageLoader } from './services/app-translation.service';
import { ConfigurationService } from './services/configuration.service';
import { AlertService } from './services/alert.service';
import { LocalStoreManager } from './services/local-store-manager.service';
import { EndpointFactory } from './services/endpoint-factory.service';
import { NotificationService } from './services/notification.service';
import { NotificationEndpoint } from './services/notification-endpoint.service';
import { AccountService } from './services/account.service';
import { AccountEndpoint } from './services/account-endpoint.service';

import { EqualValidator } from './directives/equal-validator.directive';
import { LastElementDirective } from './directives/last-element.directive';
import { AutofocusDirective } from './directives/autofocus.directive';
import { BootstrapTabDirective } from './directives/bootstrap-tab.directive';
import { BootstrapToggleDirective } from './directives/bootstrap-toggle.directive';
import { BootstrapSelectDirective } from './directives/bootstrap-select.directive';
import { BootstrapDatepickerDirective } from './directives/bootstrap-datepicker.directive';
import { GroupByPipe } from './pipes/group-by.pipe';

import { AppComponent } from "./components/app.component";
import { LoginComponent } from "./components/login/login.component";
import { HomeComponent } from "./components/home/home.component";
import { CustomersComponent } from "./components/customers/customers.component";
import { ProductsComponent } from "./components/products/products.component";
import { OrdersComponent } from "./components/orders/orders.component";
import { SettingsComponent } from "./components/settings/settings.component";
import { AboutComponent } from "./components/about/about.component";
import { NotFoundComponent } from "./components/not-found/not-found.component";

import { BannerDemoComponent } from "./components/controls/banner-demo.component";
import { TodoDemoComponent } from "./components/controls/todo-demo.component";
import { StatisticsDemoComponent } from "./components/controls/statistics-demo.component";
import { NotificationsViewerComponent } from "./components/controls/notifications-viewer.component";
import { SearchBoxComponent } from "./components/controls/search-box.component";
import { UserInfoComponent } from "./components/controls/user-info.component";
import { UserPreferencesComponent } from "./components/controls/user-preferences.component";
import { UsersManagementComponent } from "./components/controls/users-management.component";
import { RolesManagementComponent } from "./components/controls/roles-management.component";
import { RoleEditorComponent } from "./components/controls/role-editor.component";


//old
//mport { UniversalModule } from "angular2-universal";
import { HttpModule } from "@angular/http";
import { CommonModule } from "@angular/common";

import { MatIconModule} from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/';
import { FlexLayoutModule } from "@angular/flex-layout";

import {
  CovalentLoadingModule, TdLoadingFactory, CovalentDialogsModule, CovalentSearchModule, CovalentDataTableModule,
  CovalentFileModule, TdFileService } from "@covalent/core";

import { SimpleTimer } from "ng2-simple-timer";

import { TorrentComponent } from "./components/torrent/torrent.component";
import { InvokePlayerComponent } from "./components/invoke-player/invoke-player.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { UploadButtonComponent } from "./components/upload-button/upload-button.component";
import { UploadButtonUrlComponent } from "./components/upload-button-url/upload-button-url.component";
import { VideoJSComponent } from "./components/videojs/videojs.component";

import { ProgressComponent } from "./components/progress/progress.component";
import { DataPresenterComponent } from "./components/data-presenter/data-presenter.component";
import { FlowplayerComponent } from "./components/flowplayer/flowplayer.component";

import { DataService } from "./services/data.service";
import { WebSocketService } from "./services/websocket.service";
import { ContentService } from "./services/content.service";
import { TorrentProgressService } from "./services/torrent-progress.service";
import { ComponentInjectorService } from "./services/component-injector.service";

import { FilterPipe } from "./pipes/filter.pipe";
import { SortPipe } from "./pipes/sort.pipe";
import { FileSizePipe } from "./pipes/filesize.pipe";
import { ShowFilesPipe } from "./pipes/show-files.pipe";




@NgModule({
  imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        AppRoutingModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useClass: TranslateLanguageLoader
            }
        }),
        NgxDatatableModule,
        ToastyModule.forRoot(),
        TooltipModule.forRoot(),
        PopoverModule.forRoot(),
        BsDropdownModule.forRoot(),
        CarouselModule.forRoot(),
        ModalModule.forRoot(),
        ChartsModule,

        //old
        //UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.

        HttpModule,
        CommonModule,
        MatTooltipModule,
        MatIconModule,
        MatInputModule,
        FlexLayoutModule,
        CovalentDialogsModule,
        CovalentFileModule,
        CovalentSearchModule,
    CovalentDataTableModule, CovalentLoadingModule
    ],
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
        CustomersComponent,
        ProductsComponent,
        OrdersComponent,
        SettingsComponent,
        UsersManagementComponent, UserInfoComponent, UserPreferencesComponent,
        RolesManagementComponent, RoleEditorComponent,
        AboutComponent,
        NotFoundComponent,
        NotificationsViewerComponent,
        SearchBoxComponent,
        StatisticsDemoComponent, TodoDemoComponent, BannerDemoComponent,
        EqualValidator,
        LastElementDirective,
        AutofocusDirective,
        BootstrapTabDirective,
        BootstrapToggleDirective,
        BootstrapSelectDirective,
        BootstrapDatepickerDirective,
        GroupByPipe,

        //old
        TorrentComponent,
        InvokePlayerComponent,
        FlowplayerComponent,

        NavMenuComponent,
        UploadButtonComponent, UploadButtonUrlComponent, DataPresenterComponent, VideoJSComponent,
        ProgressComponent,
      FilterPipe, SortPipe, FileSizePipe, ShowFilesPipe

    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        { provide: ErrorHandler, useClass: AppErrorHandler },
        AlertService,
        ConfigurationService,
        AppTitleService,
        AppTranslationService,
        NotificationService,
        NotificationEndpoint,
        AccountService,
        AccountEndpoint,
        LocalStoreManager,
        EndpointFactory,

        //old
        DataService, ContentService, TorrentProgressService, WebSocketService, SimpleTimer,
      TdFileService, ComponentInjectorService, TdLoadingFactory
    ],
    //old
    entryComponents: [InvokePlayerComponent, FlowplayerComponent],
    bootstrap: [AppComponent]
})
export class AppModule {
}


export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}